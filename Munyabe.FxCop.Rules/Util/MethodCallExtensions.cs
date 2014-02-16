using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="MethodCall"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class MethodCallExtensions
    {
        /// <summary>
        /// メソッドが呼び出されたローカル変数、フィールドまたはプロパティを取得します。
        /// それ以外の場合は<see langword="null"/>を返します。
        /// </summary>
        /// <param name="call">呼び出されたメソッド</param>
        /// <returns>メソッドが呼び出されたローカル変数、フィールドまたはプロパティ</returns>
        public static Node GetCalledNode(this MethodCall call)
        {
            var target = GetTargetObject(call);
            if (target is Local)
            {
                return target;
            }
            else
            {
                // MEMO : フィールド
                var binding = target as MemberBinding;
                if (binding != null)
                {
                    return binding.BoundMember;
                }

                // MEMO : プロパティ
                var propertyCall = target as MethodCall;
                if (propertyCall != null)
                {
                    var propertyBinding = propertyCall.Callee as MemberBinding;
                    if (propertyBinding != null)
                    {
                        var accessor = propertyBinding.BoundMember as Method;
                        if (accessor != null && accessor.IsPropertyAccessor())
                        {
                            return accessor;
                        }
                    }
                }
            }

            return null;
        }

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

            var calledMethod = GetMethod(call);
            if (calledMethod == null)
            {
                return false;
            }

            var method = calledMethod.IsGeneric ? calledMethod.Template : calledMethod;
            return method == expectedMethod || method.IsInherit(expectedMethod);
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

        /// <summary>
        /// メソッドが呼び出された式を取得します。
        /// </summary>
        private static Expression GetTargetObject(MethodCall call)
        {
            var binding = call.Callee as MemberBinding;
            if (binding == null)
            {
                return null;
            }

            return binding.TargetObject ?? call.Operands[0];
        }
    }
}
