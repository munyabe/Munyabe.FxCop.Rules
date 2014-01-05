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
        /// 型が指定された条件を全て満たすかどうかを判定します。
        /// </summary>
        /// <param name="type">判定する型</param>
        /// <param name="flags">満たすべき条件</param>
        /// <returns>指定された条件を全て満たす場合は<see langword="true"/></returns>
        public static bool HasTypeFlags(this TypeNode type, params TypeFlags[] flags)
        {
            Guard.ArgumentNotNull(type, "type");

            return flags.All(flag => (type.Flags & flag) != 0);
        }
    }
}
