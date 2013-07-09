using System.Linq;
using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Performance
{
    public class AvoidUsingEnumerableCount : BaseRule
    {
        private static Method _countMethod;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public AvoidUsingEnumerableCount()
            : base("AvoidUsingEnumerableCount")
        {
        }

        public override ProblemCollection Check(Member member)
        {
            if (member is Method)
            {
                Visit(member);
            }

            return Problems;
        }

        public override void VisitMethodCall(MethodCall call)
        {
            string resolutionName;
            if (IsCountCall(call) && IsWrongCall(call, out resolutionName))
            {
                Problems.Add(new Problem(GetNamedResolution(resolutionName)));
            }

            base.VisitMethodCall(call);
        }

        public override void BeforeAnalysis()
        {
            base.BeforeAnalysis();
            _countMethod = GetCountMethod();
        }

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
            //var targetType = FrameworkAssemblies.SystemCore.GetType(Identifier.For("System.Linq"), Identifier.For("Enumerable"));
            //var countMethod = targetType.Members[82];
            //var countMethod2 = targetType.GetMethod(Identifier.For("Count"), paramType);
            //var countMethod3 = targetType.GetMethod(Identifier.For("System.Linq.Enumerable.Count(System.Collections.Generic.IEnumerable`1<type parameter.TSource>)"));

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
            //return template.FullName == "System.Linq.Enumerable.Count(System.Collections.Generic.IEnumerable`1<type parameter.TSource>)";
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
                resolutionName = "Array";
                return true;
            }
            else if (typeNode.IsGeneric && typeNode.Template.IsAssignableTo(FrameworkTypes.GenericICollection))
            {
                resolutionName = "ICollection";
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
