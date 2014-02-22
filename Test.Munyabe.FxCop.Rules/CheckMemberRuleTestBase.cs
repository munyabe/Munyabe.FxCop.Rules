using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.FxCop.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Munyabe.FxCop.Rules
{
    /// <summary>
    /// メンバーを解析するルールをテストする基底クラスです。
    /// </summary>
    /// <typeparam name="TRule">解析するルール</typeparam>
    [TestClass]
    public abstract class CheckMemberRuleTestBase<TRule> : RuleTestBase<TRule> where TRule : BaseIntrospectionRule, new()
    {
        /// <summary>
        /// 解析するメンバーがルールを満たしていることを検証します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        protected void AssertIsSatisfied(string memberName)
        {
            AssertIsSatisfied(GetType(), memberName);
        }

        /// <summary>
        /// 解析するメンバーがルールを満たしていることを検証します。
        /// </summary>
        /// <param name="type">解析する型</param>
        /// <param name="memberName">解析するメンバー名</param>
        protected void AssertIsSatisfied(Type type, string memberName)
        {
            Assert.AreEqual(0, GetIssues(type, memberName).Length, "The '{0}' includes violation.", memberName);
        }

        /// <summary>
        /// 解析するメンバーに違反が1つあることを検証します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        protected void AssertIsViolated(string memberName)
        {
            AssertIsViolated(memberName, 1);
        }

        /// <summary>
        /// 解析するメンバーに違反が1つあることを検証します。
        /// </summary>
        /// <param name="type">解析する型</param>
        /// <param name="memberName">解析するメンバー名</param>
        protected void AssertIsViolated(Type type, string memberName)
        {
            AssertIsViolated(type, memberName, 1);
        }

        /// <summary>
        /// 解析するメンバーに指定の数の違反があることを検証します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        /// <param name="expectedCount">期待する違反の数</param>
        protected void AssertIsViolated(string memberName, int expectedCount)
        {
            AssertIsViolated(GetType(), memberName, expectedCount);
        }

        /// <summary>
        /// 解析するメンバーに指定の数の違反があることを検証します。
        /// </summary>
        /// <param name="type">解析する型</param>
        /// <param name="memberName">解析するメンバー名</param>
        /// <param name="expectedCount">期待する違反の数</param>
        protected void AssertIsViolated(Type type, string memberName, int expectedCount)
        {
            Assert.AreEqual(expectedCount, GetIssues(type, memberName).Length);
        }

        /// <summary>
        /// 解析するメンバーに指定の原因の違反が1つあることを検証します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        /// <param name="resolutionName">違反の原因名</param>
        protected void AssertIsViolated(string memberName, string resolutionName)
        {
            AssertIsViolated(GetType(), memberName, resolutionName);
        }

        /// <summary>
        /// 解析するメンバーに指定の原因の違反が1つあることを検証します。
        /// </summary>
        /// <param name="type">解析する型</param>
        /// <param name="memberName">解析するメンバー名</param>
        /// <param name="resolutionName">違反の原因名</param>
        protected void AssertIsViolated(Type type, string memberName, string resolutionName)
        {
            var issues = GetIssues(type, memberName);

            Assert.AreEqual(1, issues.Length);
            Assert.AreEqual(resolutionName, issues[0].Attribute("Name").Value);
        }

        /// <summary>
        /// 違反を検出できないことを確認します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        protected void AssertIsNotDetectable(string memberName)
        {
            AssertIsSatisfied(memberName);
        }

        /// <summary>
        /// 違反を検出できないことを確認します。
        /// </summary>
        /// <param name="type">解析する型</param>
        /// <param name="memberName">解析するメンバー名</param>
        protected void AssertIsNotDetectable(Type type, string memberName)
        {
            AssertIsSatisfied(type, memberName);
        }

        /// <summary>
        /// 解析結果からルール違反を示す要素を取得します。
        /// </summary>
        private XElement[] GetIssues(Type type, string memberName)
        {
            if (type.GetMember(memberName).Any() == false)
            {
                var cctor = type.GetConstructors(BindingFlags.Static | BindingFlags.NonPublic).FirstOrDefault();
                if (cctor == null || cctor.Name != memberName)
                {
                    throw new ArgumentException(string.Format("The member '{0}' is not found.", memberName));
                }
            }

            var targetName = string.Format("#{0}()", memberName);
            return GetIssues(type, typeElement =>
                {
                    return typeElement.Element("Members").Elements("Member")
                        .Where(element => element.Attribute("Name").Value == targetName)
                        .FirstOrDefault();
                })
                .ToArray();
        }
    }
}
