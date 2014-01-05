using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysisTestTarget.Performance
{
    public static class AvoidEnumGetHashCodeTarget
    {
        public static void OK()
        {
            var code = ((int)Hoge.A).GetHashCode();
        }

        public static void NG()
        {
            var code = Hoge.A.GetHashCode();
        }

        public enum Hoge
        {
            A,
            B,
            C
        }
    }
}
