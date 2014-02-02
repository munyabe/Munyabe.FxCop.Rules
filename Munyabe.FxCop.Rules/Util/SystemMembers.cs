using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="System"/>名前空間のメンバーです。
    /// </summary>
    internal static class SystemMembers
    {
        /// <summary>
        /// <see cref="object.GetHashCode"/>のメソッドです。
        /// </summary>
        public static Method Object_GetHashCode = FrameworkTypes.Object.GetMethod(Identifier.For("GetHashCode"));

        /// <summary>
        /// <see cref="Enumerable.Count{T}(IEnumerable{T})"/>のメソッドです。
        /// </summary>
        public static Method Enumerable_Count = FrameworkAssemblies.SystemCore.GetType(Identifier.For("System.Linq"), Identifier.For("Enumerable"))
            .GetMembersNamed(Identifier.For("Count"))
            .OfType<Method>()
            .FirstOrDefault(method => method.Parameters.Count == 1 && method.Parameters[0].Type.IsAssignableToInstanceOf(FrameworkTypes.GenericIEnumerable));
    }
}
