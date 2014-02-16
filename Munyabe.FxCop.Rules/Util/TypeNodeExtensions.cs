using System.Linq;
using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="TypeNode"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class TypeNodeExtensions
    {
        /// <summary>
        /// 指定の名前のメソッドを取得します。
        /// </summary>
        /// <param name="type">メソッドを保持する型</param>
        /// <param name="name">メソッド名</param>
        public static Method GetMethodNamed(this TypeNode type, string name)
        {
            Guard.ArgumentNotNull(type, "type");

            return type.GetMembersNamed(Identifier.For(name))
                .OfType<Method>()
                .FirstOrDefault();
        }
    }
}
