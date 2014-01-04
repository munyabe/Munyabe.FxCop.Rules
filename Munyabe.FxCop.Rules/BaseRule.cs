using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop
{
    /// <summary>
    /// 解析ルールを実装する基底クラスです。
    /// </summary>
    public abstract class BaseRule : BaseIntrospectionRule
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="name">ルール名</param>
        public BaseRule(string name)
            : base(name, "Munyabe.FxCop.Rules", typeof(BaseRule).Assembly)
        {
        }
    }
}