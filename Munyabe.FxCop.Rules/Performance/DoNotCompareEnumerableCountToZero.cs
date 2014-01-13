using System;
using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// <see cref="Enumerable.Count"/>の結果を0と比較するコードを検出する解析ルールです。
    /// </summary>
    public class DoNotCompareEnumerableCountToZero : BaseRule
    {
        private static Method _countMethod;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public DoNotCompareEnumerableCountToZero()
            : base("DoNotCompareEnumerableCountToZero")
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
        /// 演算子の両辺が<see cref="Enumerable.Count"/>の呼び出しと0であるかどうかを判定します。
        /// </summary>
        private bool IsCountAndZeroOperation(BinaryExpression expression)
        {
            if (IsInt32Literal(expression.Operand1, 0))
            {
                return IsMethodCall(expression.Operand2, _countMethod);
            }

            if (IsInt32Literal(expression.Operand2, 0))
            {
                return IsMethodCall(expression.Operand1, _countMethod);
            }

            return false;
        }

        /// <summary>
        /// 式が<see cref="Int32"/>のリテラルかどうかを判定します。
        /// </summary>
        private static bool IsInt32Literal(Expression expression, int value)
        {
            var literal = expression as Literal;
            if (literal == null)
            {
                return false;
            }

            if (literal.Type != FrameworkTypes.Int32)
            {
                return false;
            }

            return Reference.Equals(literal.Value, value);
        }

        /// <summary>
        /// 式が指定の値のメソッドの呼び出しかどうかを判定します。
        /// </summary>
        private static bool IsMethodCall(Expression expression, Method method)
        {
            var call = expression as MethodCall;
            return call != null && call.IsCall(method);
        }
    }
}
