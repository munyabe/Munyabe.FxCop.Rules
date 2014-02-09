using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.FxCop.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Munyabe.FxCop.Rules.Properties;

namespace Test.Munyabe.FxCop.Rules
{
    /// <summary>
    /// 解析するルールをテストする基底クラスです。
    /// </summary>
    /// <typeparam name="TRule">解析するルール</typeparam>
    [TestClass]
    public abstract class RuleTestBase<TRule> where TRule : BaseIntrospectionRule, new()
    {
        /// <summary>
        /// 解析結果のファイルパスです。
        /// </summary>
        private string _resultFilePath;

        /// <summary>
        /// テスト対象のルールを取得します。
        /// </summary>
        public TRule Rule { get; private set; }

        /// <summary>
        /// 現在のテストの実行についての情報および機能を提供するテストコンテキストを取得または設定します。
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public RuleTestBase()
        {
            Rule = new TRule();
        }

        /// <summary>
        /// テストクラスを初期化します。
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var testType = GetType();
            _resultFilePath = string.Format("{0}.fxcop", testType.Name);

            if (File.Exists(_resultFilePath) == false)
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = Settings.Default.FxCopCmdPath,
                    Arguments = string.Format(@"/rule:-""{0}"" /ruleid:{1}#{2} /file:""{3}"" /types:""{4}*"" /searchgac /out:{5}",
                        typeof(TRule).Assembly.Location, Rule.Category, Rule.CheckId, testType.Assembly.Location, testType.FullName, _resultFilePath),
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                process.WaitForExit();
            }
        }

        /// <summary>
        /// 解析結果からルール違反を示す要素を取得します。
        /// </summary>
        /// <param name="type">解析する型</param>
        /// <param name="getTargetElement"><c>Type</c>要素から解析対象の要素を取得する処理</param>
        protected IEnumerable<XElement> GetIssues(Type type, Func<XElement, XElement> getTargetElement)
        {
            var result = Enumerable.Empty<XElement>();

            var typeElement = GetResultTypes(type).FirstOrDefault();
            if (typeElement == null)
            {
                return result;
            }

            var targetElement = getTargetElement(typeElement);
            if (targetElement == null)
            {
                return result;
            }

            var category = Rule.Category;
            var checkId = Rule.CheckId;

            var messageNode = targetElement.Element("Messages").Elements("Message")
                .Where(element => element.Attribute("Category").Value == category && element.Attribute("CheckId").Value == checkId)
                .FirstOrDefault();
            if (messageNode == null)
            {
                return result;
            }

            return messageNode.Elements("Issue");
        }

        /// <summary>
        /// 型の解析結果を取得します。
        /// </summary>
        /// <param name="type">解析する型</param>
        protected IEnumerable<XElement> GetResultTypes(Type type)
        {
            var report = XDocument.Load(_resultFilePath).Element("FxCopProject").Element("FxCopReport");
            if (report.HasElements == false)
            {
                return Enumerable.Empty<XElement>();
            }

            var targetTypeName = GetTargetTypeName(type);

            return report
                .Element("Targets").Element("Target")
                .Element("Modules").Element("Module")
                .Element("Namespaces").Element("Namespace")
                .Element("Types").Elements("Type")
                .Where(element => element.Attribute("Name").Value == targetTypeName);
        }

        /// <summary>
        /// 現在の<see cref="Type"/>を含めた宣言している全てのクラスを取得します。
        /// </summary>
        private static IEnumerable<Type> GetDeclaringTypes(Type type)
        {
            yield return type;

            var currentType = type.DeclaringType;
            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.DeclaringType;
            }
        }

        /// <summary>
        /// 宣言しているクラスを含めたクラス名を取得します。
        /// </summary>
        private static string GetTargetTypeName(Type type)
        {
            var types = GetDeclaringTypes(type).Select(each => each.Name).Reverse();
            return string.Join("+", types);
        }
    }
}
