using System;
using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Maintainability
{
    /// <summary>
    /// デフォルト値で初期化しているフィールドを検出する解析ルールです。
    /// </summary>
    public class DoNotInitializeFieldsToDefaultValue : BaseRule
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public DoNotInitializeFieldsToDefaultValue()
            : base("DoNotInitializeFieldsToDefaultValue")
        {
        }

        /// <inheritdoc />
        public override ProblemCollection Check(Member member)
        {
            var method = member as Method;
            if (method != null && method.IsInitializer())
            {
                CheckInitializer(method);
            }

            return Problems;
        }

        /// <inheritdoc />
        public override void VisitAssignmentStatement(AssignmentStatement assignment)
        {
            if (IsDefaultValue(assignment.Target.Type, assignment.Source))
            {
                Problems.Add(CreateProblem(assignment));
            }
        }

        /// <summary>
        /// コンストラクターを検査します。
        /// </summary>
        private void CheckInitializer(Method constructor)
        {
            var block = constructor.Body.Statements.OfType<Block>().First();

            // MEMO : フィールド初期化後に Nop が入ることでコンストラクターの処理と区別する
            block.Statements
                .TakeWhile(statement => statement.NodeType == NodeType.AssignmentStatement)
                .ForEach(Visit);
        }

        /// <summary>
        /// 指定の型のデフォルト値かどうかを判定します。
        /// </summary>
        private static bool IsDefaultValue(TypeNode type, Expression expression)
        {
            var literal = expression as Literal;
            if (literal != null)
            {
                return IsDefaultValue(type, literal);
            }

            // MEMO : long
            var unary = expression as UnaryExpression;
            if (unary != null)
            {
                var operand = unary.Operand as Literal;
                return operand != null && IsDefaultValue(type, operand);
            }

            // MEMO : decimal
            var construct = expression as Construct;
            if (construct != null)
            {
                return construct.Type == FrameworkTypes.Decimal && IsDecimalDefaultValue(construct);
            }

            return false;
        }

        /// <summary>
        /// 指定の型のデフォルト値かどうかを判定します。
        /// </summary>
        private static bool IsDefaultValue(TypeNode type, Literal literal)
        {
            if (type.IsValueType)
            {
                if (literal.Type == FrameworkTypes.Double)
                {
                    return Equals(literal.Value, 0d);
                }
                else if (literal.Type == FrameworkTypes.Single)
                {
                    return Equals(literal.Value, 0f);
                }

                return Equals(literal.Value, 0);
            }
            else
            {
                return literal.Value == null;
            }
        }

        /// <summary>
        /// <see cref="Decimal"/>のデフォルト値かどうかを判定します。
        /// </summary>
        private static bool IsDecimalDefaultValue(Construct construct)
        {
            var operands = construct.Operands;
            // MEMO : 0
            if (operands.Count == 1)
            {
                return operands[0].IsInt32Literal(0);
            }

            // MEMO : 0.XXXm
            return operands.Count == 5 &&
                operands.Take(4).All(operand => operand.IsInt32Literal(0)) &&
                operands[4].IsInt32Literal();
        }
    }
}
