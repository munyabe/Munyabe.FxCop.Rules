using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.FxCop.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Munyabe.FxCop.Rules.Util;

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
        /// 解析するメンバーに違反がないかどうか判定します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        /// <returns>違反がない場合は<see langword="true"/></returns>
        protected bool IsSuccess(string memberName)
        {
            return IsSuccessInternal(GetType(), memberName);
        }

        /// <summary>
        /// 解析するメンバーに違反がないかどうか判定します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="memberName">解析するメンバー名</param>
        /// <returns>違反がない場合は<see langword="true"/></returns>
        protected bool IsSuccess<T>(string memberName)
        {
            return IsSuccessInternal(typeof(T), memberName);
        }

        /// <summary>
        /// 解析するメンバーに違反が1つあるかどうか判定します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        /// <returns>違反が1つある場合は<see langword="true"/></returns>
        protected bool IsFailuer(string memberName)
        {
            return IsFailuer(memberName, 1);
        }

        /// <summary>
        /// 解析するメンバーに違反が1つあるかどうか判定します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="memberName">解析するメンバー名</param>
        /// <returns>違反が1つある場合は<see langword="true"/></returns>
        protected bool IsFailuer<T>(string memberName)
        {
            return IsFailuer<T>(memberName, 1);
        }

        /// <summary>
        /// 解析するメンバーに指定の原因の違反が1つあるかどうか判定します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        /// <param name="resolutionName">違反の原因名</param>
        /// <returns>指定の原因の違反が1つある場合は<see langword="true"/></returns>
        protected bool IsFailuer(string memberName, string resolutionName)
        {
            return IsFailuerInternal(GetType(), memberName, resolutionName);
        }

        /// <summary>
        /// 解析するメンバーに指定の原因の違反が1つあるかどうか判定します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="memberName">解析するメンバー名</param>
        /// <param name="resolutionName">違反の原因名</param>
        /// <returns>指定の原因の違反が1つある場合は<see langword="true"/></returns>
        protected bool IsFailuer<T>(string memberName, string resolutionName)
        {
            return IsFailuerInternal(typeof(T), memberName, resolutionName);
        }

        /// <summary>
        /// 解析するメンバーに指定の数の違反があるかどうか判定します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        /// <param name="expectedCount">期待する違反の数</param>
        /// <returns>指定の数の違反がある場合は<see langword="true"/></returns>
        protected bool IsFailuer(string memberName, int expectedCount)
        {
            return IsFailuerInternal(GetType(), memberName, expectedCount);
        }

        /// <summary>
        /// 解析するメンバーに指定の数の違反があるかどうか判定します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="memberName">解析するメンバー名</param>
        /// <param name="expectedCount">期待する違反の数</param>
        /// <returns>指定の数の違反がある場合は<see langword="true"/></returns>
        protected bool IsFailuer<T>(string memberName, int expectedCount)
        {
            return IsFailuerInternal(typeof(T), memberName, expectedCount);
        }

        /// <summary>
        /// 違反を検出できないことを確認します。
        /// </summary>
        /// <param name="memberName">解析するメンバー名</param>
        /// <returns>違反を検出できない場合は<see langword="true"/></returns>
        protected bool IsNotDetectable(string memberName)
        {
            return IsSuccess(memberName);
        }

        /// <summary>
        /// 違反を検出できないことを確認します。
        /// </summary>
        /// <typeparam name="T">解析する型</typeparam>
        /// <param name="memberName">解析するメンバー名</param>
        /// <returns>違反を検出できない場合は<see langword="true"/></returns>
        protected bool IsNotDetectable<T>(string memberName)
        {
            return IsSuccess<T>(memberName);
        }

        /// <summary>
        /// 解析結果からルール違反を示す要素を取得します。
        /// </summary>
        private IEnumerable<XElement> GetIssues(Type type, string memberName)
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
                });
        }

        /// <summary>
        /// 解析するメンバーに違反がないかどうか判定する内部メソッドです。
        /// </summary>
        private bool IsSuccessInternal(Type type, string memberName)
        {
            return GetIssues(type, memberName).Any() == false;
        }

        /// <summary>
        /// 解析するメンバーに指定の数の違反があるかどうか判定する内部メソッドです。
        /// </summary>
        private bool IsFailuerInternal(Type type, string memberName, int expectedCount)
        {
            return GetIssues(type, memberName).IsCount(expectedCount);
        }

        /// <summary>
        /// 解析するメンバーに指定の原因の違反が1つあるかどうか判定する内部メソッドです。
        /// </summary>
        protected bool IsFailuerInternal(Type type, string memberName, string resolutionName)
        {
            var issues = GetIssues(type, memberName).ToArray();
            return issues.IsCount(1) && issues.First().Attribute("Name").Value == resolutionName;
        }
    }
}
