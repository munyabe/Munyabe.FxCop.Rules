using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Performance;

namespace Test.Munyabe.FxCop.Rules.Performance
{
    /// <summary>
    /// <see cref="AvoidEnumGetHashCode"/>のルールをテストするクラスです。
    /// </summary>
    [TestClass]
    public class AvoidEnumGetHashCodeTest : CheckMemberRuleTestBase<AvoidEnumGetHashCode>
    {
        [TestMethod]
        public void SuccessTest()
        {
            AssertIsSatisfied("Success");
        }

        [TestMethod]
        public void FailuerTest()
        {
            AssertIsViolated("Failuer");
        }

        public void Success()
        {
            var code = ((int)Hoge.A).GetHashCode();
        }

        public void Failuer()
        {
            var code = Hoge.A.GetHashCode();
        }

        public enum Hoge
        {
            A,
            B,
            C
        }
    }
}
