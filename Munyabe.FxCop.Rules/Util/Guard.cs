using System;
using System.Diagnostics;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// パラメーターが契約を満たしていることを示すためのクラスです。
    /// </summary>
    [DebuggerStepThrough]
    public static class Guard
    {
        /// <summary>
        /// パラメーターが<see langword="null"/>でないことを示します。
        /// </summary>
        /// <typeparam name="T">パラメーターの型</typeparam>
        /// <param name="argumentValue">チェックするパラメーター</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/>が<see langword="null"/>です。</exception>
        public static void ArgumentNotNull<T>(T argumentValue, string argumentName) where T : class
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
