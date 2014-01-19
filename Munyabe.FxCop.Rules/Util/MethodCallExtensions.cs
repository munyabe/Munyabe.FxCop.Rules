using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="MethodCall"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class MethodCallExtensions
    {
        /// <summary>
        /// 指定のメソッドの呼び出しかどうかを判定します。
        /// </summary>
        /// <param name="call">判定するメソッド呼び出し</param>
        /// <param name="expectedMethod">期待するメソッド</param>
        /// <returns>指定のメソッドの呼び出しの場合は<see langword="true"/></returns>
        public static bool IsCall(this MethodCall call, Method expectedMethod)
        {
            Guard.ArgumentNotNull(call, "call");
            Guard.ArgumentNotNull(expectedMethod, "expectedMethod");

            var method = GetMethod(call);
            return method != null && method.Template == expectedMethod;
        }

        /// <summary>
        /// コンストラクターの呼び出しかどうかを判定します。
        /// </summary>
        /// <param name="call">判定するメソッド呼び出し</param>
        /// <returns>コンストラクターの呼び出しの場合は<see langword="true"/></returns>
        public static bool IsInitializerCall(this MethodCall call)
        {
            Guard.ArgumentNotNull(call, "call");

            var method = GetMethod(call);
            return method != null && method.IsInitializer();
        }

        /// <summary>
        /// 呼び出すメソッドを取得します。
        /// </summary>
        private static Method GetMethod(MethodCall call)
        {
            var member = call.Callee as MemberBinding;
            if (member == null)
            {
                return null;
            }

            return member.BoundMember as Method;
        }
    }
}
