namespace CodeAnalysisTestTarget.Performance
{
    /// <summary>
    /// <c>AvoidEnumGetHashCode</c>の解析ルールを確認するためのクラスです。
    /// </summary>
    public class AvoidEnumGetHashCodeTarget
    {
        public void OK()
        {
            var code = ((int)Hoge.A).GetHashCode();
        }

        public void NG()
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
