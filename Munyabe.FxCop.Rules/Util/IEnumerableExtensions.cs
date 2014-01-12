using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素に対して、指定された処理を実行します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="action">各要素に対して実行するアクション</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="action"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(action, "action");

            foreach (T each in source)
            {
                action(each);
            }
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>から<see cref="HashSet{T}"/>を作成します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <returns>作成した<see cref="HashSet{T}"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            Guard.ArgumentNotNull(source, "source");

            return new HashSet<T>(source);
        }
    }
}
