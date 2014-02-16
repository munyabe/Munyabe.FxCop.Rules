using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Performance;

namespace Test.Munyabe.FxCop.Rules.Performance
{
    /// <summary>
    /// <see cref="AvoidUnnecessaryEnumerableCount"/>のルールをテストするクラスです。
    /// </summary>
    [TestClass]
    public class AvoidUnnecessaryEnumerableCountTest : CheckMemberRuleTestBase<AvoidUnnecessaryEnumerableCount>
    {
        [TestMethod]
        public void SuccessTest()
        {
            AssertIsSatisfied("Success");
        }

        [TestMethod]
        public void ArrayTest()
        {
            AssertIsViolated("Array", AvoidUnnecessaryEnumerableCount.ArrayResolutionName);
        }

        [TestMethod]
        public void ICollectionTest()
        {
            AssertIsViolated("ICollection", AvoidUnnecessaryEnumerableCount.ICollectionResolutionName);
        }

        public void Success()
        {
            var enumerable = Enumerable.Range(0, 3);
            enumerable.Count();
        }

        public void Array()
        {
            var array = new int[] { 1, 2, 4 };
            array.Count();
        }

        public void ICollection()
        {
            var list = new List<int> { 1, 2, 4 };
            list.Count();
        }
    }
}
