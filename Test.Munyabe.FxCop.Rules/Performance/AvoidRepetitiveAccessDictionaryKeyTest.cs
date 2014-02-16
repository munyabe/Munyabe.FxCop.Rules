using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Performance;

namespace Test.Munyabe.FxCop.Rules.Performance
{
    /// <summary>
    /// <see cref="AvoidRepetitiveAccessDictionaryKey"/>のルールをテストするクラスです。
    /// </summary>
    [TestClass]
    public class AvoidRepetitiveAccessDictionaryKeyTest : CheckMemberRuleTestBase<AvoidRepetitiveAccessDictionaryKey>
    {
        [TestMethod]
        public void SuccessTest()
        {
            AssertIsSatisfied("Success");
        }

        [TestMethod]
        public void CallMethodTest()
        {
            AssertIsSatisfied("CallMethod");
        }

        [TestMethod]
        public void AssignmentInBetweenTest()
        {
            AssertIsSatisfied("AssignmentInBetween");
        }

        [TestMethod]
        public void CallLocalVariableTest()
        {
            AssertIsViolated("CallLocalVariable");
        }

        [TestMethod]
        public void CallFieldTest()
        {
            AssertIsViolated("CallField");
        }

        [TestMethod]
        public void CallPropertyTest()
        {
            AssertIsViolated("CallProperty");
        }

        [TestMethod]
        public void TernaryExpressionTest()
        {
            AssertIsViolated("TernaryExpression");
        }

        [TestMethod]
        public void CallTwoVariablesTest()
        {
            AssertIsViolated("CallTwoVariables", 2);
        }

        [TestMethod]
        public void CallInMethodParamTest()
        {
            AssertIsViolated("CallInMethodParam");
        }

        [TestMethod]
        public void AccessConstantKeyTest()
        {
            AssertIsViolated("AccessConstantKey");
        }

        private readonly IDictionary<int, string> _field = new Dictionary<int, string>();

        public IDictionary<int, string> Property { get; set; }

        public string Success()
        {
            var dictionary = new Dictionary<int, string>();

            string value;
            if (dictionary.TryGetValue(1, out value))
            {
                return value;
            }

            return string.Empty;
        }

        public string CallMethod()
        {
            var key = 1;
            if (GetValues().ContainsKey(key))
            {
                return GetValues()[key];
            }

            return string.Empty;
        }

        public string AssignmentInBetween()
        {
            var dictionary = new Dictionary<int, string>();
            var key = 1;
            if (dictionary.ContainsKey(key))
            {
                dictionary = new Dictionary<int, string>();
                return dictionary[key];
            }

            return string.Empty;
        }

        public static string CallLocalVariable()
        {
            var dictionary = new Dictionary<int, string>();
            var key = 1;
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return string.Empty;
        }

        public string CallField()
        {
            var key = 1;
            if (_field.ContainsKey(key))
            {
                return _field[key];
            }

            return string.Empty;
        }

        public string CallProperty()
        {
            var key = 1;
            if (Property.ContainsKey(key))
            {
                return Property[key];
            }

            return string.Empty;
        }

        public string CallTwoVariables()
        {
            var dictionary = new Dictionary<int, string>();
            var dictionary2 = new Dictionary<int, string>();

            var key = 1;
            if (dictionary.ContainsKey(key) && dictionary2.ContainsKey(key))
            {
                var value = dictionary[key];
                var value2 = dictionary2[key];

                return value + value2;
            }

            return string.Empty;
        }

        public string CallInMethodParam()
        {
            var dictionary = new Dictionary<int, string>();
            var key = 1;
            if (dictionary.ContainsKey(key))
            {
                return string.Concat("NG" + dictionary[key]);
            }

            return string.Empty;
        }

        public string TernaryExpression()
        {
            var dictionary = new Dictionary<int, string>();

            var key = 1;
            return dictionary.ContainsKey(key) ? dictionary[key] : string.Empty;
        }

        public static string AccessConstantKey()
        {
            var dictionary = new Dictionary<int, string>();
            if (dictionary.ContainsKey(1))
            {
                return dictionary[1];
            }

            return string.Empty;
        }

        public string AnotherClass()
        {
            var dictionary = new ConcurrentDictionary<int, string>();
            var key = 1;
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return string.Empty;
        }

        private IDictionary<int, string> GetValues()
        {
            return new Dictionary<int, string>();
        }
    }
}
