using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// 同じ値に対して最初の要素を繰り返し列挙していることを検出する解析ルールです。
    /// </summary>
    public class AvoidRepetitiveFirstEnumeration : AvoidRepetitiveMethodCallBase
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public AvoidRepetitiveFirstEnumeration()
            : base("AvoidRepetitiveFirstEnumeration")
        {
        }

        /// <inheritdoc />
        protected override bool IsFirstTargetMethodCall(MethodCall call)
        {
            return call.IsCall(SystemMembers.Enumerable_Any);
        }

        /// <inheritdoc />
        protected override bool IsSecondTargetMethodCall(MethodCall call)
        {
            return call.IsCall(SystemMembers.Enumerable_First) || call.IsCall(SystemMembers.Enumerable_FirstOrDefault);
        }
    }
}
