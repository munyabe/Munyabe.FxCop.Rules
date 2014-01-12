using System.Linq;
using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// <see cref="Enumerable"/>を解析するクラスです。
    /// </summary>
    public static class EnumerableAnalyst
    {
        private const string NAME_SPACE = "System.Linq";
        private const string TYPE_NAME = "Enumerable";

        /// <summary>
        /// <see cref="Enumerable.Count"/>メソッドを取得します。
        /// </summary>
        public static Method GetCountMethod()
        {
            var paramType = FrameworkTypes.GenericIEnumerable;

            return FrameworkAssemblies.SystemCore.GetType(Identifier.For(NAME_SPACE), Identifier.For(TYPE_NAME))
                .GetMembersNamed(Identifier.For("Count"))
                .OfType<Method>()
                .FirstOrDefault(method => method.Parameters.Count == 1 && method.Parameters[0].Type.IsAssignableToInstanceOf(paramType));
        }
    }
}
