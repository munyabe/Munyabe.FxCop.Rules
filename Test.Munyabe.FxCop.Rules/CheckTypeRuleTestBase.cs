using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.FxCop.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Munyabe.FxCop.Rules
{
    /// <summary>
    /// 型を解析するルールをテストする基底クラスです。
    /// </summary>
    /// <typeparam name="TRule">解析するルール</typeparam>
    [TestClass]
    public abstract class CheckTypeRuleTestBase<TRule> : RuleTestBase<TRule> where TRule : BaseIntrospectionRule, new()
    {
        /// <summary>
        /// 解析する型がルールを満たしていることを検証します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        protected void AssertIsSatisfied<T>()
        {
            Assert.AreEqual(0, GetIssues<T>().Length, "The '{0}' is violated.", typeof(T).Name);
        }

        /// <summary>
        /// 解析する型に違反が1つあることを検証します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        protected void AssertIsViolated<T>()
        {
            AssertIsViolated<T>(1);
        }

        /// <summary>
        /// 解析する型に指定の数の違反があることを検証します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="expectedCount">期待する違反の数</param>
        protected void AssertIsViolated<T>(int expectedCount)
        {
            Assert.AreEqual(expectedCount, GetIssues<T>().Count());
        }

        /// <summary>
        /// 解析する型に指定の原因の違反が1つあることを検証します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="memberName">解析する型</param>
        /// <param name="resolutionName">違反の原因名</param>
        protected void AssertIsViolated<T>(string resolutionName)
        {
            var issues = GetIssues<T>().ToArray();

            Assert.AreEqual(1, issues.Length);
            Assert.AreEqual(resolutionName, issues[0].Attribute("Name").Value);
        }

        /// <summary>
        /// 違反を検出できないことを確認します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        protected void AssertIsNotDetectable<T>()
        {
            AssertIsSatisfied<T>();
        }

        /// <summary>
        /// 解析結果からルール違反を示す要素を取得します。
        /// </summary>
        private XElement[] GetIssues<T>()
        {
            return GetIssues(typeof(T), element => element).ToArray();
        }
    }
}
