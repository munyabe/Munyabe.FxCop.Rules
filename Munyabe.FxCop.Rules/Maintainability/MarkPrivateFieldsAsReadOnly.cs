using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Maintainability
{
    /// <summary>
    /// readonly にできるフィールドを検出する解析ルールです。
    /// </summary>
    public class MarkPrivateFieldsAsReadOnly : BaseRule
    {
        private readonly HashSet<Expression> _assignmentFields = new HashSet<Expression>();
        private readonly HashSet<Expression> _refParamFields = new HashSet<Expression>();

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public MarkPrivateFieldsAsReadOnly()
            : base("MarkPrivateFieldsAsReadOnly")
        {
        }

        /// <inheritdoc />
        public override ProblemCollection Check(TypeNode type)
        {
            var fields = type.Members.OfType<Field>().Where(IsPossibleDefinition).ToArray();
            if (fields.Any())
            {
                Visit(type);

                fields
                    .Where(field => _assignmentFields.Contains(field.Name) == false &&
                        _refParamFields.Contains(field.Name) == false)
                    .ForEach(field => Problems.Add(new Problem(GetResolution(field.DeclaringType.Name.Name, field.Name.Name))));
            }

            return Problems;
        }

        /// <inheritdoc />
        public override void VisitAssignmentStatement(AssignmentStatement assignment)
        {
            base.VisitAssignmentStatement(assignment);

            var field = GetAssignmentField(assignment);
            if (field != null)
            {
                _assignmentFields.Add(field.Name);
            }
        }

        /// <inheritdoc />
        public override void VisitMethodCall(MethodCall call)
        {
            call.Operands
                .Where(operand => operand.Type is Reference)
                .OfType<UnaryExpression>()
                .Select(operand => operand.Operand)
                .OfType<MemberBinding>()
                .Select(memberBinding => memberBinding.BoundMember)
                .OfType<Field>()
                .ForEach(field => _refParamFields.Add(field.Name));
        }

        /// <summary>
        /// 設定されるフィールドを取得します。フィールドの設定でない場合は<see langword="null"/>が返却されます。
        /// </summary>
        private static Field GetAssignmentField(AssignmentStatement assignment)
        {
            var memberBinding = assignment.Target as MemberBinding;
            if (memberBinding == null)
            {
                return null;
            }

            var field = memberBinding.BoundMember as Field;
            if (field == null)
            {
                return null;
            }

            // MEMO : 静的フィールド
            if (memberBinding.TargetObject == null)
            {
                return field;
            }

            var thisObject = memberBinding.TargetObject as This;
            if (thisObject != null && thisObject.DeclaringMethod.IsInitializer() == false)
            {
                return field;
            }

            if (IsClosureBinding(assignment.Source) || IsClosureBinding(memberBinding.TargetObject))
            {
                return field;
            }

            // MEMO : 三項演算子、インクリメント演算子
            if (memberBinding.TargetObject.NodeType == NodeType.Pop)
            {
                return field;
            }

            // MEMO : オブジェクト初期化子
            var local = memberBinding.TargetObject as Local;
            if (local != null && local.Name.Name.StartsWith("<>g__initLocal"))
            {
                return field;
            }

            return null;
        }

        /// <summary>
        /// クロージャーの自動生成クラスかどうかを判定します。
        /// </summary>
        private static bool IsClosure(TypeNode type)
        {
            return type.Name.Name.StartsWith("<>c__DisplayClass") && type.IsPrivate && type.IsStatic;
        }

        /// <summary>
        /// クロージャーの自動生成クラス内で設定する式かどうかを判定します。
        /// </summary>
        private static bool IsClosureBinding(Expression expression)
        {
            var binding = expression as MemberBinding;
            return binding != null && binding.TargetObject != null && IsClosure(binding.TargetObject.Type);
        }

        /// <summary>
        /// イベントの自動生成フィールドかどうかを判定します。
        /// </summary>
        private static bool IsEvent(Field field)
        {
            var members = field.Type.Members;

            return field.Type is DelegateNode &&
                members.Count == 4 &&
                IsMethod(members[0] as InstanceInitializer, ".ctor", FrameworkTypes.Void, FrameworkTypes.Object, FrameworkTypes.IntPtr) &&
                IsMethod(members[1] as Method, "Invoke", FrameworkTypes.Void) &&
                IsMethod(members[2] as Method, "BeginInvoke", SystemTypes.IAsyncResult) &&
                IsMethod(members[3] as Method, "EndInvoke", FrameworkTypes.Void, SystemTypes.IAsyncResult);
        }

        /// <summary>
        /// ラムダ式をキャッシュする自動生成フィールドかどうかを判定します。
        /// </summary>
        private static bool IsLambdaCache(Field field)
        {
            return field.Name.Name.StartsWith("CS$<>9__") && field.Type.IsAssignableTo(FrameworkTypes.Delegate);
        }

        /// <summary>
        /// 指定のメソッドかどうかを判定します。
        /// </summary>
        private static bool IsMethod(Method method, string name, TypeNode returnType)
        {
            return method.Name.Name == name && method.ReturnType == returnType;
        }

        /// <summary>
        /// 指定のメソッドかどうかを判定します。
        /// </summary>
        private static bool IsMethod(Method method, string name, TypeNode returnType, params TypeNode[] parameters)
        {
            return IsMethod(method, name, returnType) &&
                method.Parameters.Count == parameters.Length &&
                method.Parameters.Select(param => param.Type).SequenceEqual(parameters);
        }

        /// <summary>
        /// フィールドの定義から readonly を付与できる候補か判定します。
        /// </summary>
        private bool IsPossibleDefinition(Field field)
        {
            return field.IsPrivate &&
                field.IsLiteral == false &&
                field.IsInitOnly == false &&
                IsLambdaCache(field) == false &&
                IsEvent(field) == false;
        }
    }
}
