using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.FxCop.Performance;

namespace Test.Munyabe.FxCop.Rules.Performance
{
    /// <summary>
    /// <see cref="AvoidUnnecessaryFieldInitialization"/>のルールをテストするクラスです。
    /// </summary>
    [TestClass]
    public class AvoidUnnecessaryFieldInitializationTest : CheckMemberRuleTestBase<AvoidUnnecessaryFieldInitialization>
    {
        [TestMethod]
        public void FieldInitializeTest()
        {
            AssertIsSatisfied(typeof(FieldInitialize), ".ctor");
        }

        [TestMethod]
        public void StaticFieldInitializeTest()
        {
            AssertIsSatisfied(typeof(StaticFieldInitialize), ".cctor");
        }

        [TestMethod]
        public void ConstructorInitializeTest()
        {
            AssertIsSatisfied(typeof(ConstructorInitialize), ".ctor");
        }

        [TestMethod]
        public void StaticConstructorInitializeTest()
        {
            AssertIsSatisfied(typeof(StaticConstructorInitialize), ".cctor");
        }

        [TestMethod]
        public void NotInitializeTest()
        {
            AssertIsSatisfied(typeof(NotInitialize), ".ctor");
        }

        [TestMethod]
        public void OtherInitializeTest()
        {
            AssertIsSatisfied(typeof(OtherInitialize), ".ctor");
        }

        [TestMethod]
        public void SetByLambdaTest()
        {
            AssertIsSatisfied(typeof(SetByLambda), ".ctor");
        }

        [TestMethod]
        public void UnnecessaryFieldInitializeTest()
        {
            AssertIsViolated(typeof(UnnecessaryFieldInitialize), ".ctor", 18);
        }

        [TestMethod]
        public void UnnecessaryStaticFieldInitializeTest()
        {
            AssertIsViolated(typeof(UnnecessaryStaticFieldInitialize), ".cctor");
        }

        [TestMethod]
        public void UnnecessaryConstructorInitializeTest()
        {
            AssertIsViolated(typeof(UnnecessaryConstructorInitialize), ".ctor");
        }

        public class FieldInitialize
        {
            private readonly byte _byte = 1;
            private readonly short _short = 1;
            private readonly int _int = 1;

            private readonly uint _uint = 1;
            private readonly uint _uintSuffix = 1u;

            private readonly long _long = 1;
            private readonly long _longSuffix = 1L;

            private readonly ulong _ulong = 1;
            private readonly ulong _ulongSuffix = 1UL;

            private readonly float _float = 1;
            private readonly float _floatSuffix = 1.0f;

            private readonly double _double = 1;
            private readonly double _doubleSuffix = 1.0d;

            private readonly decimal _decimal = 1;
            private readonly decimal _decimalSuffix = 1.0m;

            private readonly bool _bool = true;

            private readonly char _char = 'a';
            private readonly string _string = "OK";
        }

        public class StaticFieldInitialize
        {
            private static readonly string _string = "OK";
            private static readonly string _initializedConstructor = string.Empty;
        }

        public class ConstructorInitialize
        {
            private readonly string _field;

            public ConstructorInitialize()
            {
                _field = null;
            }
        }

        public class StaticConstructorInitialize
        {
            private static readonly string _field;

            static StaticConstructorInitialize()
            {
                _field = null;
            }
        }

        public class NotInitialize
        {
            private readonly byte _byte;
            private readonly short _short;
            private readonly int _int;
            private readonly uint _uint;
            private readonly long _long;
            private readonly ulong _ulong;
            private readonly float _float;
            private readonly double _double;
            private readonly decimal _decimal;
            private readonly bool _bool;
            private readonly char _char;
            private readonly string _string;
        }

        public class OtherInitialize
        {
            private readonly DateTime _dateTime = new DateTime();
            private const int CONST = 0;

            private readonly string _initializedMethod = GetDefaultValue();
            private string _initializedConstructor = string.Empty;

            public int Property { get; set; }

            public OtherInitialize()
            {
                Property = 0;
                Initialize();
            }

            public void Initialize()
            {
                _initializedConstructor = null;
            }

            private static string GetDefaultValue()
            {
                return null;
            }
        }

        public class SetByLambda
        {
            private string _field;

            public SetByLambda()
            {
                EventHandler handler = (sender, e) => _field = "OK";
            }
        }

        public class UnnecessaryFieldInitialize
        {
            private readonly byte _byte = 0;
            private readonly short _short = 0;
            private readonly int _int = 0;

            private readonly uint _uint = 0;
            private readonly uint _uintSuffix = 0u;

            private readonly long _long = 0;
            private readonly long _longSuffix = 0L;

            private readonly ulong _ulong = 0;
            private readonly ulong _ulongSuffix = 0UL;

            private readonly float _float = 0;
            private readonly float _floatSuffix = 0.0f;

            private readonly double _double = 0;
            private readonly double _doubleSuffix = 0.0d;

            private readonly decimal _decimal = 0;
            private readonly decimal _decimalSuffix = 0.0m;

            private readonly bool _bool = false;

            private readonly char _char = char.MinValue;
            private readonly string _string = null;
        }

        public class UnnecessaryStaticFieldInitialize
        {
            private static readonly string _string = null;
        }

        public class UnnecessaryConstructorInitialize
        {
            private readonly string _field = null;

            public UnnecessaryConstructorInitialize()
            {
                _field = "NG";
            }
        }
    }
}
