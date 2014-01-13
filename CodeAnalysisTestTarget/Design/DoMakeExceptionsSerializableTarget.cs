using System;

namespace CodeAnalysisTestTarget.Design
{
    /// <summary>
    /// <c>DoMakeExceptionsSerializable</c>の解析ルールを確認するためのクラスです。
    /// </summary>
    public class DoMakeExceptionsSerializableTarget
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
