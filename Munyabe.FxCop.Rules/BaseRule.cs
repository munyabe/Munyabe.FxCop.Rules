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
        protected BaseRule(string name)
            : base(name, "Munyabe.FxCop.Rules", typeof(BaseRule).Assembly)
        {
        }

        /// <summary>
        /// 指定のソースコードの情報を設定した<see cref="Problem"/>を作成します。
        /// </summary>
        protected Problem CreateProblem(Node node)
        {
            return CreateProblem(GetResolution(), node);
        }

        /// <summary>
        /// 指定のソースコードの情報を設定した<see cref="Problem"/>を作成します。
        /// </summary>
        protected Problem CreateProblem(string resolutionName, Node node)
        {
            return CreateProblem(GetNamedResolution(resolutionName), node);
        }

        /// <summary>
        /// 指定のソースコードの情報を設定した<see cref="Problem"/>を作成します。
        /// </summary>
        private static Problem CreateProblem(Resolution resolution, Node node)
        {
            return new Problem(resolution)
            {
                SourceFile = node.SourceContext.FileName,
                SourceLine = node.SourceContext.StartLine
            };
        }
    }
}