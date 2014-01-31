using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Design
{
    /// <summary>
    /// シリアル化可能でない例外を検出する解析ルールです。
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
            if (type.IsAssignableTo(FrameworkTypes.Exception) && type.IsAbstract == false && RuleUtilities.IsSerializable(type) == false)
            {
                Problems.Add(new Problem(GetResolution(type.Name.Name)));
            }

            return Problems;
        }
    }
}
