using System.Linq;
using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// <see cref="AvoidUnnecessaryEnumerableCount"/>の解析ルールです。
    /// </summary>
    public class AvoidUnnecessaryEnumerableCount : BaseRule
    {
        private const string RESOLUTION_NAME_ARRAY = "Array";
        private const string RESOLUTION_NAME_ICOLLECTION = "ICollection";
        private static Method _countMethod;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public AvoidUnnecessaryEnumerableCount()
            : base("AvoidUnnecessaryEnumerableCount")
        {
        }

        /// <inheritdoc />
        public override ProblemCollection Check(Member member)
        {
            if (member is Method)
            {
                Visit(member);
            }

            return Problems;
        }

        /// <inheritdoc />
        public override void VisitMethodCall(MethodCall call)
        {
            string resolutionName;
            if (IsCountCall(call) && IsWrongCall(call, out resolutionName))
            {
                Problems.Add(new Problem(GetNamedResolution(resolutionName)));
            }

            base.VisitMethodCall(call);
        }

        /// <inheritdoc />
        public override void BeforeAnalysis()
        {
            base.BeforeAnalysis();
            _countMethod = GetCountMethod();
        }

        /// <inheritdoc />
        public override void AfterAnalysis()
        {
            _countMethod = null;
            base.AfterAnalysis();
        }

        /// <summary>
        /// <see cref="Enumerable.Count"/>のインスタンスを取得します。
        /// </summary>
        private static Method GetCountMethod()
        {
            var paramType = FrameworkTypes.GenericIEnumerable;

            return FrameworkAssemblies.SystemCore.GetType(Identifier.For("System.Linq"), Identifier.For("Enumerable"))
                .GetMembersNamed(Identifier.For("Count"))
                .OfType<Method>()
                .FirstOrDefault(method => method.Parameters.Count == 1 && method.Parameters[0].Type.IsAssignableToInstanceOf(paramType));
        }

        /// <summary>
        /// <see cref="Enumerable.Count"/>の呼び出しかどうかを判定します。
        /// </summary>
        private static bool IsCountCall(MethodCall call)
        {
            var member = call.Callee as MemberBinding;
            if (member == null)
            {
                return false;
            }

            var method = member.BoundMember as Method;
            if (method == null)
            {
                return false;
            }

            return method.Template == _countMethod;
        }

        /// <summary>
        /// 誤った<see cref="Enumerable.Count"/>の呼び出しかどうかを判定します。
        /// </summary>
        private static bool IsWrongCall(MethodCall call, out string resolutionName)
        {
            resolutionName = string.Empty;

            var operand = call.Operands.FirstOrDefault();
            if (operand == null)
            {
                return false;
            }

            var typeNode = operand.Type;
            if (typeNode.IsAssignableTo(FrameworkTypes.Array))
            {
                resolutionName = RESOLUTION_NAME_ARRAY;
                return true;
            }
            else if (typeNode.IsGeneric && typeNode.Template.IsAssignableTo(FrameworkTypes.GenericICollection))
            {
                resolutionName = RESOLUTION_NAME_ICOLLECTION;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
