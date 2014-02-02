using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// <see cref="Enumerable.Count{T}(IEnumerable{T})"/>の結果を0と比較するコードを検出する解析ルールです。
    /// </summary>
    public class DoNotCompareEnumerableCountToZero : BaseRule
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public DoNotCompareEnumerableCountToZero()
            : base("DoNotCompareEnumerableCountToZero")
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
        public override void VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            var operand1 = binaryExpression.Operand1 as BinaryExpression;
            if (operand1 == null)
            {
                var isEqual = binaryExpression.NodeType == NodeType.Ceq;
                var isLessThan = binaryExpression.NodeType == NodeType.Clt;
                var isGreaterThan = binaryExpression.NodeType == NodeType.Cgt;

                if (isEqual || isLessThan || isGreaterThan)
                {
                    if (IsCountAndZeroOperation(binaryExpression))
                    {
                        Problems.Add(CreateProblem(binaryExpression));
                    }
                }
            }
            else
            {
                var isNotEqual = operand1.NodeType == NodeType.Ceq;
                var isLessThanEqual = operand1.NodeType == NodeType.Cgt;
                var isGreaterThanEqual = operand1.NodeType == NodeType.Clt;

                if (isNotEqual || isLessThanEqual || isGreaterThanEqual)
                {
                    if (IsCountAndZeroOperation(operand1))
                    {
                        Problems.Add(CreateProblem(binaryExpression));
                    }
                }
            }
        }

        /// <summary>
        /// 演算子の両辺が<see cref="Enumerable.Count{T}(IEnumerable{T})"/>の呼び出しと0であるかどうかを判定します。
        /// </summary>
        private bool IsCountAndZeroOperation(BinaryExpression expression)
        {
            if (expression.Operand1.IsInt32Literal(0))
            {
                return expression.Operand2.IsMethodCall(SystemMembers.Enumerable_Count);
            }

            if (expression.Operand2.IsInt32Literal(0))
            {
                return expression.Operand1.IsMethodCall(SystemMembers.Enumerable_Count);
            }

            return false;
        }
    }
}
