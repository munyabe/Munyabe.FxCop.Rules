using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Design;

namespace Test.Munyabe.FxCop.Rules.Design
{
    /// <summary>
    /// <see cref="DoMakeExceptionsSerializable"/>のルールをテストするクラスです。
    /// </summary>
    [TestClass]
    public class DoMakeExceptionsSerializableTest : CheckTypeRuleTestBase<DoMakeExceptionsSerializable>
    {
        [TestMethod]
        public void SerializableExceptionTest()
        {
            Assert.IsTrue(IsSuccess<SerializableException>());
        }

        [TestMethod]
        public void AbstractExceptionTest()
        {
            Assert.IsTrue(IsSuccess<AbstractException>());
        }

        [TestMethod]
        public void NotExceptionTest()
        {
            Assert.IsTrue(IsSuccess<NotException>());
        }

        [TestMethod]
        public void UnserializableExceptionTest()
        {
            Assert.IsTrue(IsFailuer<UnserializableException>());
        }

        [TestMethod]
        public void ConcreteExceptionTest()
        {
            Assert.IsTrue(IsFailuer<ConcreteException>());
        }

        [Serializable]
        public class SerializableException : Exception
        {
        }

        public abstract class AbstractException : Exception
        {
        }

        public class NotException
        {
        }

        public class UnserializableException : Exception
        {
        }

        public class ConcreteException : AbstractException
        {
        }
    }
}
