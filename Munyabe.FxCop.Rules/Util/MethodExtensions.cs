using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="Method"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class MethodExtensions
    {
        /// <summary>
        /// コンストラクターがどうかを判定します。
        /// </summary>
        /// <param name="method">判定するメソッド</param>
        /// <returns>コンストラクターの場合は<see langword="true"/></returns>
        public static bool IsInitializer(this Method method)
        {
            Guard.ArgumentNotNull(method, "method");

            return method is InstanceInitializer || method is StaticInitializer;
        }

        /// <summary>
        /// 自動生成されたプロパティのメソッドかどうかを判定します。
        /// </summary>
        /// <param name="method">判定するメソッド</param>
        /// <returns>プロパティのメソッドの場合は<see langword="true"/></returns>
        public static bool IsPropertyAccessor(this Method method)
        {
            Guard.ArgumentNotNull(method, "method");

            var name = method.Name.Name;
            return name.StartsWith("get_") || name.StartsWith("set_");
        }
    }
}
