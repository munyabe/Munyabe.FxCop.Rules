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
            AssertIsSatisfied(typeof(SerializableException));
        }

        [TestMethod]
        public void AbstractExceptionTest()
        {
            AssertIsSatisfied(typeof(AbstractException));
        }

        [TestMethod]
        public void NotExceptionTest()
        {
            AssertIsSatisfied(typeof(NotException));
        }

        [TestMethod]
        public void UnserializableExceptionTest()
        {
            AssertIsViolated(typeof(UnserializableException));
        }

        [TestMethod]
        public void ConcreteExceptionTest()
        {
            AssertIsViolated(typeof(ConcreteException));
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
