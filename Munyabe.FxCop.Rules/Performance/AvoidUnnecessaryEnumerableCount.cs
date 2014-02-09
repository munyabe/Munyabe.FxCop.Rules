using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// 不要な<see cref="Enumerable.Count{T}(IEnumerable{T})"/>の呼び出しを検出する解析ルールです。
    /// </summary>
    public class AvoidUnnecessaryEnumerableCount : BaseRule
    {
        /// <summary>
        /// ルール違反の原因が配列であることを示すカテゴリーです。
        /// </summary>
        public const string ArrayResolutionName = "Array";

        /// <summary>
        /// ルール違反の原因がコレクションであることを示すカテゴリーです。
        /// </summary>
        public const string ICollectionResolutionName = "ICollection";

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
            if (call.IsCall(SystemMembers.Enumerable_Count) && IsWrongCall(call, out resolutionName))
            {
                Problems.Add(CreateProblem(resolutionName, call));
            }
        }

        /// <summary>
        /// 誤った<see cref="Enumerable.Count{T}(IEnumerable{T})"/>の呼び出しかどうかを判定します。
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
                resolutionName = ArrayResolutionName;
                return true;
            }
            else if (typeNode.IsGeneric && typeNode.Template.IsAssignableTo(FrameworkTypes.GenericICollection))
            {
                resolutionName = ICollectionResolutionName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
