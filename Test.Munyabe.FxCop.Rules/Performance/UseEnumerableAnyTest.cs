using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Performance;

namespace Test.Munyabe.FxCop.Rules.Performance
{
    /// <summary>
    /// <see cref="UseEnumerableAny"/>のルールをテストするクラスです。
    /// </summary>
    [TestClass]
    public class UseEnumerableAnyTest : CheckMemberRuleTestBase<UseEnumerableAny>
    {
        [TestMethod]
        public void SuccessTest()
        {
            AssertIsSatisfied("Success");
        }

        [TestMethod]
        public void FailuerTest()
        {
            AssertIsViolated("Failuer", 12);
        }

        private readonly IEnumerable<int> _field = Enumerable.Range(0, 3);

        public void Success()
        {
            var eql = _field.Count() == 1;
            var let = _field.Count() < 1;
            var grt = _field.Count() > 1;
            var neq = _field.Count() != 1;
            var lte = _field.Count() <= 1;
            var gte = _field.Count() >= 1;
        }

        public void Failuer()
        {
            var eql1 = _field.Count() == 0;
            var let1 = _field.Count() < 0;
            var grt1 = _field.Count() > 0;
            var neq1 = _field.Count() != 0;
            var lte1 = _field.Count() <= 0;
            var gte1 = _field.Count() >= 0;

            var eql2 = 0 == _field.Count();
            var let2 = 0 < _field.Count();
            var grt2 = 0 > _field.Count();
            var neq2 = 0 != _field.Count();
            var lte2 = 0 <= _field.Count();
            var gte2 = 0 >= _field.Count();
        }
    }
}
