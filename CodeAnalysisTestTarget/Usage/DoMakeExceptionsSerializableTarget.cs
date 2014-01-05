using System;

namespace CodeAnalysisTestTarget.Usage
{
    public static class DoMakeExceptionsSerializableTarget
    {
        [Serializable]
        public class OKException : Exception
        {
        }

        public abstract class OKAbstractException : Exception
        {
        }

        public class OKNotException
        {
        }

        public class NGException : Exception
        {
        }

        public class NGConcreteException : OKAbstractException
        {
        }
    }
}
