using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Performance;

namespace Test.Munyabe.FxCop.Rules.Performance
{
    /// <summary>
    /// <see cref="AvoidRepetitiveFirstEnumeration"/>のルールをテストするクラスです。
    /// </summary>
    [TestClass]
    public class AvoidRepetitiveFirstEnumerationTest : CheckMemberRuleTestBase<AvoidRepetitiveFirstEnumeration>
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
        public void CallFirstOrDefaultTest()
        {
            AssertIsViolated("CallFirstOrDefault");
        }

        private readonly IEnumerable<string> _field = Enumerable.Empty<string>();

        public IEnumerable<string> Property { get; set; }

        public string Success()
        {
            var loacl = Enumerable.Empty<string>();
            var first = loacl.FirstOrDefault();
            if (first != null)
            {
                return first;
            }

            return string.Empty;
        }

        public string CallMethod()
        {
            if (GetValues().Any())
            {
                return GetValues().First();
            }

            return string.Empty;
        }

        public string AssignmentInBetween()
        {
            var local = Enumerable.Empty<string>();
            if (local.Any())
            {
                local = new[] { "OK" };
                return local.First();
            }

            return string.Empty;
        }

        public static string CallLocalVariable()
        {
            var loacl = Enumerable.Empty<string>();
            if (loacl.Any())
            {
                return loacl.First();
            }

            return string.Empty;
        }

        public string CallField()
        {
            if (_field.Any())
            {
                return _field.First();
            }

            return string.Empty;
        }

        public string CallProperty()
        {
            if (Property.Any())
            {
                return Property.First();
            }

            return string.Empty;
        }

        public string CallTwoVariables()
        {
            var local = Enumerable.Empty<string>();
            var local2 = Enumerable.Empty<string>();

            if (local.Any() && local2.Any())
            {
                var first = local.First();
                var first2 = local2.First();

                return first + first2;
            }

            return string.Empty;
        }

        public string CallInMethodParam()
        {
            var local = Enumerable.Empty<string>();
            if (local.Any())
            {
                return string.Concat("NG" + local.First());
            }

            return string.Empty;
        }

        public string TernaryExpression()
        {
            var loacl = Enumerable.Empty<string>();
            return loacl.Any() ? loacl.First() : string.Empty;
        }

        public static string CallFirstOrDefault()
        {
            var loacl = Enumerable.Empty<string>();
            if (loacl.Any())
            {
                return loacl.FirstOrDefault();
            }

            return string.Empty;
        }

        private IEnumerable<string> GetValues()
        {
            return Enumerable.Empty<string>();
        }
    }
}
