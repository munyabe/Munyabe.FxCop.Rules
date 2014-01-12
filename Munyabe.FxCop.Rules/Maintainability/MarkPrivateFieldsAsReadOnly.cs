using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Maintainability
{
    /// <summary>
    /// readonly にできるフィールドを検出する解析ルールです。
    /// </summary>
    public class MarkPrivateFieldsAsReadOnly : BaseRule
    {
        private readonly FieldInfo _offsetField = typeof(Expression).GetField("ILOffset", BindingFlags.NonPublic | BindingFlags.Instance);
        private readonly HashSet<Field> _setFields = new HashSet<Field>();

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
                    .Where(field => _setFields.Contains(field) == false)
                    .ForEach(field => Problems.Add(new Problem(GetResolution(field.Name.Name))));
            }

            return Problems;
        }

        /// <inheritdoc />
        public override void VisitMemberBinding(MemberBinding memberBinding)
        {
            var boundField = memberBinding.BoundMember as Field;
            if (boundField != null)
            {
                var targetObject = memberBinding.TargetObject as This;
                if (targetObject != null && targetObject.DeclaringMethod.IsInitializer() == false && IsSetBinding(memberBinding))
                {
                    _setFields.Add(boundField);
                }
            }
        }

        /// <summary>
        /// フィールドの定義から readonly を付与できる候補か判定します。
        /// </summary>
        private bool IsPossibleDefinition(Field field)
        {
            return field.IsLiteral == false && field.IsPrivate && field.IsInitOnly == false;
        }

        /// <summary>
        /// 値の設定どうかを判定します。
        /// </summary>
        private bool IsSetBinding(MemberBinding memberBinding)
        {
            var offset = _offsetField.GetValue(memberBinding);
            return (int)offset == 0;
        }
    }
}
