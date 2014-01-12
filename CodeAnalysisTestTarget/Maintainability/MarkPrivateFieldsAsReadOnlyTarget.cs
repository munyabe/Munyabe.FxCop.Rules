using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeAnalysisTestTarget.Maintainability
{
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

        public class NG_InitializedField
        {
            private string _field = "OK";

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
                _field = "OK";
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
    }
}
