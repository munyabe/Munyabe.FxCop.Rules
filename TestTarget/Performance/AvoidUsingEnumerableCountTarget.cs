using System.Collections.Generic;
using System.Linq;

namespace TestTarget.Performance
{
    public static class AvoidUsingEnumerableCountTarget
    {
        public static void OK()
        {
            var enumerable = Enumerable.Range(0, 3);
            enumerable.Count();
        }

        public static void NG_ICollection()
        {
            var list = new List<int> { 1, 2, 3 };
            list.Count();
        }

        public static void NG_Array()
        {
            var array = new int[] { 1, 2, 3 };
            array.Count();
        }
    }
}
