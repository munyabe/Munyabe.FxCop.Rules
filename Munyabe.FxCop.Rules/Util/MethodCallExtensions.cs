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
            var member = call.Callee as MemberBinding;
            if (member == null)
            {
                return false;
            }

            var boundMethod = member.BoundMember as Method;
            if (boundMethod == null)
            {
                return false;
            }

            return boundMethod.Template == expectedMethod;
        }
    }
}
