using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.FxCop.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Munyabe.FxCop.Rules.Util;

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
        /// 解析する型に違反がないかどうか判定します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <returns>違反がない場合は<see langword="true"/></returns>
        protected bool IsSuccess<T>()
        {
            return GetIssues<T>().Any() == false;
        }

        /// <summary>
        /// 解析する型に違反が1つあるかどうか判定します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <returns>違反が1つある場合は<see langword="true"/></returns>
        protected bool IsFailuer<T>()
        {
            return IsFailuer<T>(1);
        }

        /// <summary>
        /// 解析する型に指定の原因の違反が1つあるかどうか判定します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="memberName">解析する型</param>
        /// <param name="resolutionName">違反の原因名</param>
        /// <returns>指定の原因の違反が1つある場合は<see langword="true"/></returns>
        protected bool IsFailuer<T>(string resolutionName)
        {
            var issues = GetIssues<T>().ToArray();
            return issues.IsCount(1) && issues.First().Attribute("Name").Value == resolutionName;
        }

        /// <summary>
        /// 解析する型に指定の数の違反があるかどうか判定します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="expectedCount">期待する違反の数</param>
        /// <returns>指定の数の違反がある場合は<see langword="true"/></returns>
        protected bool IsFailuer<T>(int expectedCount)
        {
            return GetIssues<T>().IsCount(expectedCount);
        }

        /// <summary>
        /// 違反を検出できないことを確認します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <returns>違反を検出できない場合は<see langword="true"/></returns>
        protected bool IsNotDetectable<T>()
        {
            return IsSuccess<T>();
        }

        /// <summary>
        /// 解析結果からルール違反を示す要素を取得します。
        /// </summary>
        private IEnumerable<XElement> GetIssues<T>()
        {
            return GetIssues(typeof(T), element => element);
        }
    }
}
