using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// 不要な<see cref="Enumerable.Count"/>の呼び出しを検出する解析ルールです。
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
        public override void BeforeAnalysis()
        {
            _countMethod = EnumerableAnalyst.GetCountMethod();
        }

        /// <inheritdoc />
        public override void AfterAnalysis()
        {
            _countMethod = null;
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
            if (call.IsCall(_countMethod) && IsWrongCall(call, out resolutionName))
            {
                Problems.Add(CreateProblem(resolutionName, call));
            }
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
