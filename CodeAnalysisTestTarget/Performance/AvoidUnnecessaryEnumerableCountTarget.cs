using System.Collections.Generic;
using System.Linq;

namespace CodeAnalysisTestTarget.Performance
{
    /// <summary>
    /// <c>AvoidUnnecessaryEnumerableCount</c>の解析ルールを確認するためのクラスです。
    /// </summary>
    public class AvoidUnnecessaryEnumerableCountTarget
    {
        public void OK()
        {
            var enumerable = Enumerable.Range(0, 3);
            enumerable.Count();
        }

        public void NG_ICollection()
        {
            var list = new List<int> { 1, 2, 3 };
            list.Count();
        }

        public void NG_Array()
        {
            var array = new int[] { 1, 2, 3 };
            array.Count();
        }
    }
}
