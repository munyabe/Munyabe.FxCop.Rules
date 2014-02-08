using System;

namespace CodeAnalysisTestTarget.Performance
{
    /// <summary>
    /// <c>AvoidUnnecessaryFieldInitialization</c>の解析ルールを確認するためのクラスです。
    /// </summary>
    public class AvoidUnnecessaryFieldInitializationTarget
    {
        public class OK_FieldInitialize
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

        public class OK_StaticFieldInitialize
        {
            private static readonly string _string = "OK";
            private static readonly string _initializedConstructor = string.Empty;
        }

        public class OK_ConstructorInitialize
        {
            private readonly string _field;

            public OK_ConstructorInitialize()
            {
                _field = null;
            }
        }

        public class OK_StaticConstructorInitialize
        {
            private static readonly string _field;

            static OK_StaticConstructorInitialize()
            {
                _field = null;
            }
        }

        public class OK_NotInitialize
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

        public class OK_OtherInitialize
        {
            private readonly DateTime _dateTime = new DateTime();
            private const int CONST = 0;

            private readonly string _initializedMethod = GetDefaultValue();
            private string _initializedConstructor = string.Empty;

            public int Property { get; set; }

            public OK_OtherInitialize()
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

        public class OK_SetLambda
        {
            private string _field;

            public OK_SetLambda()
            {
                EventHandler handler = (sender, e) => _field = "OK";
            }
        }

        public class NG_FieldInitialize
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

        public class NG_StaticFieldInitialize
        {
            private static readonly string _string = null;
        }

        public class NG_ConstructorInitialize
        {
            private readonly string _field = null;

            public NG_ConstructorInitialize()
            {
                _field = "NG";
            }
        }
    }
}
