using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Maintainability;

namespace Test.Munyabe.FxCop.Rules.Maintainability
{
    /// <summary>
    /// <see cref="MarkPrivateFieldsAsReadOnly"/>のルールをテストするクラスです。
    /// </summary>
    [TestClass]
    public class MarkPrivateFieldsAsReadOnlyTest : CheckTypeRuleTestBase<MarkPrivateFieldsAsReadOnly>
    {
        [TestMethod]
        public void ReadonlyFieldTest()
        {
            Assert.IsTrue(IsSuccess<ReadonlyField>());
        }

        [TestMethod]
        public void SetFieldTest()
        {
            Assert.IsTrue(IsSuccess<SetField>());
        }

        [TestMethod]
        public void StaticFieldTest()
        {
            Assert.IsTrue(IsSuccess<StaticField>());
        }

        [TestMethod]
        public void ConstMemberTest()
        {
            Assert.IsTrue(IsSuccess<ConstMember>());
        }

        [TestMethod]
        public void PublicPropertyTest()
        {
            Assert.IsTrue(IsSuccess<PublicProperty>());
        }

        [TestMethod]
        public void PrivateSetPropertyTest()
        {
            Assert.IsTrue(IsSuccess<PrivateSetProperty>());
        }

        [TestMethod]
        public void GenericClassTest()
        {
            Assert.IsTrue(IsSuccess<GenericClass<string>>());
        }

        [TestMethod]
        public void AbstractClassTest()
        {
            Assert.IsTrue(IsSuccess<AbstractClass>());
        }

        [TestMethod]
        public void ParcialClassTest()
        {
            Assert.IsTrue(IsSuccess<ParcialClass>());
        }

        [TestMethod]
        public void CanNotMarkReadonlyFieldTest()
        {
            Assert.IsTrue(IsSuccess<CanNotMarkReadonlyField>());
        }

        [TestMethod]
        public void IncrementOperatorTest()
        {
            Assert.IsTrue(IsSuccess<IncrementOperator>());
        }

        [TestMethod]
        public void TernaryExpressionTest()
        {
            Assert.IsTrue(IsSuccess<TernaryExpression>());
        }

        [TestMethod]
        public void NullTernaryExpressionTest()
        {
            Assert.IsTrue(IsSuccess<NullTernaryExpression>());
        }

        [TestMethod]
        public void ObjectInitializerTest()
        {
            Assert.IsTrue(IsSuccess<ObjectInitializer>());
        }

        [TestMethod]
        public void RefParamMethodTest()
        {
            Assert.IsTrue(IsSuccess<RefParamMethod>());
        }

        [TestMethod]
        public void RefParamNotVoidMethodTest()
        {
            Assert.IsTrue(IsSuccess<RefParamNotVoidMethod>());
        }

        [TestMethod]
        public void EventTest()
        {
            Assert.IsTrue(IsSuccess<Event>());
        }

        [TestMethod]
        public void LambdaCacheTest()
        {
            Assert.IsTrue(IsSuccess<LambdaCache>());
        }

        [TestMethod]
        public void SetByLambdaTest()
        {
            Assert.IsTrue(IsSuccess<SetByLambda>());
        }

        [TestMethod]
        public void DelegateClosureTest()
        {
            Assert.IsTrue(IsSuccess<DelegateClosure>());
        }

        [TestMethod]
        public void LambdaClosureTest()
        {
            Assert.IsTrue(IsSuccess<LambdaClosure>());
        }

        [TestMethod]
        public void SetAndYieldReturnTest()
        {
            Assert.IsTrue(IsSuccess<SetAndYieldReturn>());
        }

        [TestMethod]
        public void InitializedFieldTest()
        {
            Assert.IsTrue(IsFailuer<InitializedField>());
        }

        [TestMethod]
        public void ConstructorInitializedFieldTest()
        {
            Assert.IsTrue(IsFailuer<ConstructorInitializedField>());
        }

        [TestMethod]
        public void NotSetFieldTest()
        {
            Assert.IsTrue(IsFailuer<NotSetField>());
        }

        [TestMethod]
        public void NotSetStaticFieldTest()
        {
            Assert.IsTrue(IsFailuer<NotSetStaticField>());
        }

        [TestMethod]
        public void NotSetGenericClassTest()
        {
            Assert.IsTrue(IsFailuer<NotSetGenericClass<string>>());
        }

        [TestMethod]
        public void DelegateFieldTest()
        {
            Assert.IsTrue(IsFailuer<DelegateField>());
        }

        [TestMethod]
        public void GetByLambdaTest()
        {
            Assert.IsTrue(IsFailuer<GetByLambda>());
        }

        [TestMethod]
        public void ConstructorTernaryExpressionTest()
        {
            Assert.IsTrue(IsNotDetectable<ConstructorTernaryExpression>());
        }

        [TestMethod]
        public void YieldReturnTest()
        {
            Assert.IsTrue(IsFailuer<YieldReturn>());
        }

        public class ReadonlyField
        {
            private readonly string _field = "OK";

            public string GetValue()
            {
                return _field;
            }
        }

        public class SetField
        {
            private string _field;

            public void SetValue(string value)
            {
                _field = value;
            }
        }

        public class StaticField
        {
            private static string _field;

            public static void SetValue(string value)
            {
                _field = value;
            }
        }

        public class ConstMember
        {
            private const string CONST = "OK";
        }

        public class PublicProperty
        {
            public string Property { get; set; }
        }

        public class PrivateSetProperty
        {
            public string Property { get; private set; }

            public PrivateSetProperty()
            {
                Property = "OK";
            }
        }

        public class GenericClass<T>
        {
            private string _field;

            public void SetValue(string value)
            {
                _field = value;
            }
        }

        public abstract class AbstractClass
        {
            private string _field;

            public void SetValue(string value)
            {
                _field = value;
            }
        }

        public partial class ParcialClass
        {
            private string _field;
        }

        partial class ParcialClass
        {
            public void SetValue(string value)
            {
                _field = value;
            }
        }

        public class CanNotMarkReadonlyField
        {
            private string _field;

            public CanNotMarkReadonlyField()
            {
                Initialized();
            }

            public void Initialized()
            {
                _field = "OK";
            }
        }

        public class IncrementOperator
        {
            private int _field;

            public void SetValue(string value)
            {
                _field++;
            }
        }

        public class TernaryExpression
        {
            private string _field;

            public string GetValue()
            {
                return string.IsNullOrEmpty(_field) ? "Empty" : (_field = string.Empty);
            }
        }

        public class NullTernaryExpression
        {
            private string _field;

            public string GetValue()
            {
                return _field ?? (_field = string.Empty);
            }
        }

        public class ObjectInitializer
        {
            private string _field;

            public ObjectInitializer CreateInstance()
            {
                return new ObjectInitializer { _field = "OK" };
            }
        }

        public class RefParamMethod
        {
            private string _field;

            public void Initialize()
            {
                SetField(ref _field, "OK");
            }

            private static void SetField(ref string field, string value)
            {
                field = value;
            }
        }

        public class RefParamNotVoidMethod
        {
            private string _field;

            public string Initialize()
            {
                return SetField(ref _field, "OK");
            }

            private static string SetField(ref string field, string value)
            {
                field = value;
                return value;
            }
        }

        public class Event
        {
            public delegate void CustomEventHandler();

            public event EventHandler Testing;
            public event EventHandler<EventArgs> GenericTesting;
            public event CustomEventHandler CustomEvent;
        }

        public class LambdaCache
        {
            public int[] GetEvenValues(int max)
            {
                return Enumerable.Range(0, max).Where(num => num % 2 == 0).ToArray();
            }
        }

        public class SetByLambda
        {
            private string _field;

            public void SetValue(string value)
            {
                new List<string> { value }.ForEach(each => _field = each);
            }
        }

        public class DelegateClosure
        {
            private string _field;

            public event EventHandler Testing;

            public DelegateClosure()
            {
                EventHandler handler = null;
                handler = (sender, e) =>
                {
                    Testing -= handler;
                    _field = "OK";
                };
                Testing += handler;
            }
        }

        public class LambdaClosure
        {
            private string _field;

            public void SetValue(string value)
            {
                new List<int> { 0 }.ForEach(each => _field = value);
            }
        }

        public class SetAndYieldReturn
        {
            private string _field;

            public IEnumerable<string> GetValues()
            {
                yield return _field = string.Empty;
            }
        }

        public class InitializedField
        {
            private string _field = "NG";

            public string GetValue()
            {
                return _field;
            }
        }

        public class ConstructorInitializedField
        {
            private string _field;

            public ConstructorInitializedField()
            {
                _field = "NG";
            }

            public string GetValue()
            {
                return _field;
            }
        }

        public class NotSetField
        {
            private List<string> _field = new List<string>();

            public void AddValue(string value)
            {
                _field.Add(value);
            }
        }

        public class NotSetStaticField
        {
            private static string _field;

            public static string GetValue()
            {
                return _field;
            }
        }

        public class NotSetGenericClass<T>
        {
            private string _field = "NG";

            public string GetValue()
            {
                return _field;
            }
        }

        public class DelegateField
        {
            private Delegate _delegate;
        }

        public class GetByLambda
        {
            private string _field;

            public void SetValue(List<string> values)
            {
                values.ForEach(value => value = _field);
            }
        }

        public class YieldReturn
        {
            private string _field;

            public IEnumerable<string> GetValues()
            {
                yield return _field;
            }
        }

        public class ConstructorTernaryExpression
        {
            private string _field;

            public ConstructorTernaryExpression()
            {
                var temp = string.IsNullOrEmpty(_field) ? "Empty" : (_field = string.Empty);
            }
        }
    }
}
