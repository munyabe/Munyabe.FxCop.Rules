using System;
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
        /// <param name="type">解析する型</param>
        protected void AssertIsSatisfied(Type type)
        {
            Assert.AreEqual(0, GetIssues(type).Length, "The '{0}' is violated.", type.Name);
        }

        /// <summary>
        /// 解析する型に違反が1つあることを検証します。
        /// </summary>
        /// <param name="type">解析する型</param>
        protected void AssertIsViolated(Type type)
        {
            AssertIsViolated(type, 1);
        }

        /// <summary>
        /// 解析する型に指定の数の違反があることを検証します。
        /// </summary>
        /// <param name="type">解析する型</param>
        /// <param name="expectedCount">期待する違反の数</param>
        protected void AssertIsViolated(Type type, int expectedCount)
        {
            Assert.AreEqual(expectedCount, GetIssues(type).Length);
        }

        /// <summary>
        /// 解析する型に指定の原因の違反が1つあることを検証します。
        /// </summary>
        /// <param name="type">解析する型</param>
        /// <param name="memberName">解析する型</param>
        /// <param name="resolutionName">違反の原因名</param>
        protected void AssertIsViolated(Type type, string resolutionName)
        {
            var issues = GetIssues(type).ToArray();

            Assert.AreEqual(1, issues.Length);
            Assert.AreEqual(resolutionName, issues[0].Attribute("Name").Value);
        }

        /// <summary>
        /// 違反を検出できないことを確認します。
        /// </summary>
        /// <param name="type">解析する型</param>
        protected void AssertIsNotDetectable(Type type)
        {
            AssertIsSatisfied(type);
        }

        /// <summary>
        /// 解析結果からルール違反を示す要素を取得します。
        /// </summary>
        private XElement[] GetIssues(Type type)
        {
            return GetIssues(type, element => element).ToArray();
        }
    }
}
