using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop
{
    public abstract class BaseRule : BaseIntrospectionRule
    {
        public BaseRule(string name)
            : base(name, "Munyabe.FxCop.Rules", typeof(BaseRule).Assembly)
        {
        }
    }
}