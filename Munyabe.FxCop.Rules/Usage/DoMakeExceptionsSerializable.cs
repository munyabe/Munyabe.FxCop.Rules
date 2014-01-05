using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Usage
{
    /// <summary>
    /// <see cref="DoMakeExceptionsSerializable"/>を検出する解析ルールです。
    /// </summary>
    public class DoMakeExceptionsSerializable : BaseRule
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public DoMakeExceptionsSerializable()
            : base("DoMakeExceptionsSerializable")
        {
        }

        /// <inheritdoc />
        public override ProblemCollection Check(TypeNode type)
        {
            if (type.IsAssignableTo(FrameworkTypes.Exception) && type.HasTypeFlags(TypeFlags.Abstract) == false && type.HasTypeFlags(TypeFlags.Serializable) == false)
            {
                Problems.Add(new Problem(GetResolution()));
            }

            return Problems;
        }
    }
}
