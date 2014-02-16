using System.Collections.Generic;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// 同じ<see cref="IDictionary{T1, T2}"/>に対して同じキーのアクセスを繰り返し列挙していることを検出する解析ルールです。
    /// </summary>
    public class AvoidRepetitiveAccessDictionaryKey : AvoidRepetitiveMethodCallBase
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public AvoidRepetitiveAccessDictionaryKey()
            : base("AvoidRepetitiveAccessDictionaryKey")
        {
        }

        /// <inheritdoc />
        protected override bool IsFirstTargetMethodCall(MethodCall call)
        {
            return call.IsCall(SystemMembers.IDictionary_ContainsKey);
        }

        /// <inheritdoc />
        protected override bool IsSecondTargetMethodCall(MethodCall call)
        {
            return call.IsCall(SystemMembers.IDictionary_GetIndexer);
        }
    }
}
