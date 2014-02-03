using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeAnalysisTestTarget.Maintainability
{
    /// <summary>
    /// <c>MarkPrivateFieldsAsReadOnly</c>の解析ルールを確認するためのクラスです。
    /// </summary>
    public class MarkPrivateFieldsAsReadOnlyTarget
    {
        public class OK_ReadonlyField
        {
            private readonly string _field = "OK";

            public string GetValue()
            {
                return _field;
            }
        }

        public class OK_SetField
        {
            private string _field;

            public void SetValue(string value)
            {
                _field = value;
            }
        }

        public class OK_StaticField
        {
            private static string _field;

            public static void SetValue(string value)
            {
                _field = value;
            }
        }

        public class OK_GenericClass<T>
        {
            private string _field;

            public void SetValue(string value)
            {
                _field = value;
            }
        }

        public abstract class OK_AbstractClass
        {
            private string _field;

            public void SetValue(string value)
            {
                _field = value;
            }
        }

        public partial class OK_ParcialClass
        {
            private string _field;
        }

        partial class OK_ParcialClass
        {
            public void SetValue(string value)
            {
                _field = value;
            }
        }

        public class OK_CanNotMarkReadonlyField
        {
            private string _field;

            public OK_CanNotMarkReadonlyField()
            {
                Initialized();
            }

            public void Initialized()
            {
                _field = "OK";
            }
        }

        public class OK_IncrementOperator
        {
            private int _field;

            public void SetValue(string value)
            {
                _field++;
            }
        }

        public class OK_TernaryExpression
        {
            private string _field;

            public string GetValue()
            {
                return string.IsNullOrEmpty(_field) ? "Empty" : (_field = string.Empty);
            }
        }

        public class OK_NullTernaryExpression
        {
            private string _field;

            public string GetValue()
            {
                return _field ?? (_field = string.Empty);
            }
        }

        public class OK_ObjectInitializer
        {
            private string _field;

            public OK_ObjectInitializer CreateInstance()
            {
                return new OK_ObjectInitializer
                {
                    _field = "OK",
                };
            }
        }

        public class OK_RefParamMethod
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

        public class OK_RefParamNotVoidMethod
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

        public class OK_ConstMember
        {
            private const string CONST = "OK";
        }

        public class OK_Property
        {
            public string Property
            {
                get { return "OK"; }
            }
        }

        public class OK_PrivateSetProperty
        {
            public string Property { get; private set; }

            public OK_PrivateSetProperty()
            {
                Property = "OK";
            }
        }

        public class OK_Event
        {
            public delegate void CustomEventHandler();

            public event EventHandler Testing;
            public event EventHandler<EventArgs> GenericTesting;
            public event CustomEventHandler CustomEvent;
        }

        public class OK_LambdaCache
        {
            public int[] GetEvenValues(int max)
            {
                return Enumerable.Range(0, max).Where(num => num % 2 == 0).ToArray();
            }
        }

        public class OK_SetLambda
        {
            private string _field;

            public void SetValue(string value)
            {
                new List<string> { value }.ForEach(each => _field = each);
            }
        }

        public class OK_DelegateClosure
        {
            private string _field;

            public event EventHandler Testing;

            public OK_DelegateClosure()
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

        public class OK_LambdaClosure
        {
            private string _field;

            public void SetValue(string value)
            {
                new List<int> { 0 }.ForEach(each => _field = value);
            }
        }

        public class OK_YieldReturn
        {
            private string _field;

            public IEnumerable<string> GetValues()
            {
                yield return _field = string.Empty;
            }
        }

        public class NG_InitializedField
        {
            private string _field = "NG";

            public string GetValue()
            {
                return _field;
            }
        }

        public class NG_ConstructorInitializedField
        {
            private string _field;

            public NG_ConstructorInitializedField()
            {
                _field = "NG";
            }

            public string GetValue()
            {
                return _field;
            }
        }

        public class NG_NotSetField
        {
            private List<string> _field = new List<string>();

            public void AddValue(string value)
            {
                _field.Add(value);
            }
        }

        public class NG_StaticField
        {
            private static string _field;

            public static string GetValue()
            {
                return _field;
            }
        }

        public class NG_GenericClass<T>
        {
            private string _field = "NG";

            public string GetValue()
            {
                return _field;
            }
        }

        public class NG_Delegate
        {
            private Delegate _delegate;
        }

        public class NG_GetLambda
        {
            private string _field;

            public void SetValue(List<string> values)
            {
                values.ForEach(value => value = _field);
            }
        }

        public class NG_YieldReturn
        {
            private string _field;

            public IEnumerable<string> GetValues()
            {
                yield return _field;
            }
        }

        public class Tolerate_ConstructorTernaryExpression
        {
            private string _field;

            public Tolerate_ConstructorTernaryExpression()
            {
                var temp = string.IsNullOrEmpty(_field) ? "Empty" : (_field = string.Empty);
            }
        }
    }
}
