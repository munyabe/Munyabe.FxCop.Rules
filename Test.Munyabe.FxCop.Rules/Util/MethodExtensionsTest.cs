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
            var genericMethod = FrameworkTypes.GenericDictionary.GetTemplateInstance(FrameworkTypes.Object, FrameworkTypes.Int32, FrameworkTypes.String)
                .GetProperty(Identifier.For("Item"), FrameworkTypes.Int32).Getter;
            var genericTemplateMethod = FrameworkTypes.GenericDictionary
                .GetProperty(Identifier.For("Item"), FrameworkTypes.GenericDictionary.TemplateParameters[0]).Getter;

            var interfaceMethod = FrameworkTypes.GenericIDictionary.GetTemplateInstance(FrameworkTypes.Object, FrameworkTypes.Int32, FrameworkTypes.String)
                .GetProperty(Identifier.For("Item"), FrameworkTypes.Int32).Getter;
            var interfaceTemplateMethod = FrameworkTypes.GenericIDictionary
                .GetProperty(Identifier.For("Item"), FrameworkTypes.GenericIDictionary.TemplateParameters[0]).Getter;

            Assert.IsTrue(genericMethod.IsInherit(interfaceMethod));
            Assert.IsTrue(genericMethod.IsInherit(interfaceTemplateMethod));
            Assert.IsTrue(genericTemplateMethod.IsInherit(interfaceTemplateMethod));
            Assert.IsTrue(interfaceMethod.IsInherit(interfaceTemplateMethod));
        }

        [TestMethod]
        public void IsInitializerTest()
        {
            Assert.IsTrue(FrameworkTypes.Object.GetMethod(Identifier.For(".ctor")).IsInitializer());
            Assert.IsTrue(FrameworkTypes.DateTime.GetMethod(Identifier.For(".ctor"), FrameworkTypes.Int64).IsInitializer());
            Assert.IsTrue(FrameworkTypes.DateTime.GetMethod(Identifier.For(".cctor")).IsInitializer());

            Assert.IsFalse(FrameworkTypes.Object.GetMethod(Identifier.For("GetHashCode")).IsInitializer());
            Assert.IsFalse(FrameworkTypes.String.GetMethod(Identifier.For("IsNullOrEmpty"), FrameworkTypes.String).IsInitializer());
        }

        [TestMethod]
        public void IsPropertyAccessorTest()
        {
            var capacity = FrameworkTypes.GenericList.GetProperty(Identifier.For("Capacity"));
            Assert.IsTrue(capacity.Getter.IsPropertyAccessor());
            Assert.IsTrue(capacity.Setter.IsPropertyAccessor());

            Assert.IsFalse(FrameworkTypes.String.GetMethod(Identifier.For(".ctor"), FrameworkTypes.Char, FrameworkTypes.Int32).IsPropertyAccessor());
            Assert.IsFalse(FrameworkTypes.String.GetMethod(Identifier.For("IsNullOrEmpty"), FrameworkTypes.String).IsPropertyAccessor());
        }
    }
}
