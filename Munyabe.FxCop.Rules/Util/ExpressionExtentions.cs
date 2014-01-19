using System;
using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="Expression"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 式が<see cref="Int32"/>のリテラルのノードかどうかを判定します。
        /// </summary>
        /// <param name="expression">判定する式</param>
        /// <returns>式が<see cref="Int32"/>のリテラルのノードの場合は<see langword="true"/></returns>
        public static bool IsInt32Literal(this Expression expression)
        {
            Guard.ArgumentNotNull(expression, "expression");

            var literal = expression as Literal;
            if (literal == null)
            {
                return false;
            }

            return literal.Type == FrameworkTypes.Int32;
        }

        /// <summary>
        /// 式が<see cref="Int32"/>のリテラルのノードかどうかを判定します。
        /// </summary>
        /// <param name="expression">判定する式</param>
        /// <param name="expectedValue">期待する値</param>
        /// <returns>式が<see cref="Int32"/>の指定した値のリテラルのノードの場合は<see langword="true"/></returns>
        public static bool IsInt32Literal(this Expression expression, int expectedValue)
        {
            Guard.ArgumentNotNull(expression, "expression");

            var literal = expression as Literal;
            if (literal == null)
            {
                return false;
            }

            return literal.Type == FrameworkTypes.Int32 && Equals(literal.Value, expectedValue);
        }

        /// <summary>
        /// 式が指定のメソッドの呼び出しかどうかを判定します。
        /// </summary>
        /// <param name="expression">判定する式</param>
        /// <param name="method">期待するメソッド</param>
        /// <returns>式が指定のメソッドの呼び出しの場合は<see langword="true"/></returns>
        public static bool IsMethodCall(this Expression expression, Method expectedMethod)
        {
            Guard.ArgumentNotNull(expression, "expression");
            Guard.ArgumentNotNull(expectedMethod, "expectedMethod");

            var call = expression as MethodCall;
            return call != null && call.IsCall(expectedMethod);
        }
    }
}
