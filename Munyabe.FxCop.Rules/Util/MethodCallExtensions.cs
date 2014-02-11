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

            if (method == null)
            {
                return false;
            }

            return method.IsGeneric ? method.Template == expectedMethod : method == expectedMethod;
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
        /// プロパティの呼び出しかどうかを判定します。
        /// </summary>
        /// <param name="call">判定するメソッド呼び出し</param>
        /// <returns>プロパティの呼び出しの場合は<see langword="true"/></returns>
        public static bool IsPropertyCall(this MethodCall call)
        {
            Guard.ArgumentNotNull(call, "call");

            if (call != null)
            {
                var propertyBinding = call.Callee as MemberBinding;
                if (propertyBinding != null)
                {
                    var accessor = propertyBinding.BoundMember as Method;
                    return accessor != null && accessor.IsPropertyAccessor();
                }
            }

            return false;
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
