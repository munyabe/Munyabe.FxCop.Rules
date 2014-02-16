using Microsoft.FxCop.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Util;

namespace Test.Munyabe.FxCop.Rules.Util
{
    /// <summary>
    /// <see cref="MethodExtensions"/>をテストするクラスです。
    /// </summary>
    [TestClass]
    public class MethodExtensionsTest
    {
        [TestMethod]
        public void IsInheritTest()
        {
            var genericMethod = FrameworkTypes.GenericDictionary
                .GetTemplateInstance(FrameworkTypes.Object, FrameworkTypes.Int32, FrameworkTypes.String).GetMethodNamed("get_Item");
            var genericTemplateMethod = FrameworkTypes.GenericDictionary.GetMethodNamed("get_Item");
            var interfaceMethod = FrameworkTypes.GenericIDictionary
                .GetTemplateInstance(FrameworkTypes.Object, FrameworkTypes.Int32, FrameworkTypes.String).GetMethodNamed("get_Item");
            var interfaceTemplateMethod = FrameworkTypes.GenericIDictionary.GetMethodNamed("get_Item");

            Assert.IsTrue(genericMethod.IsInherit(interfaceMethod));
            Assert.IsTrue(genericMethod.IsInherit(interfaceTemplateMethod));
            Assert.IsTrue(genericTemplateMethod.IsInherit(interfaceTemplateMethod));
            Assert.IsTrue(interfaceMethod.IsInherit(interfaceTemplateMethod));
        }
    }
}
