using System;
using System.Collections.Generic;
using System.Diagnostics;
using Munyabe.FxCop.Util;

namespace Test.Munyabe.FxCop.Rules.Util
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 要素の数が指定の数と一致するかどうかを判断します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">カウントする要素が格納されているシーケンス</param>
        /// <param name="count">期待する要素数</param>
        /// <returns>要素の数が指定の数と一致するとき<see langword="true"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static bool IsCount<T>(this IEnumerable<T> source, int count)
        {
            Guard.ArgumentNotNull(source, "source");

            if (count < 0)
            {
                return false;
            }

            var index = 0;
            using (var sourceEnumerator = source.GetEnumerator())
            {
                while (sourceEnumerator.MoveNext())
                {
                    index++;
                    if (count < index)
                    {
                        return false;
                    }
                }
            }

            return index == count;
        }
    }
}
