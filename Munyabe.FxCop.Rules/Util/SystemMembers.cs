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
        public static Method Enumerable_Count = GetOneParamEnumerableMethod("Count");

        /// <summary>
        /// <see cref="Enumerable.Any{T}(IEnumerable{T})"/>のメソッドです。
        /// </summary>
        public static Method Enumerable_Any = GetOneParamEnumerableMethod("Any");

        /// <summary>
        /// <see cref="Enumerable.First{T}(IEnumerable{T})"/>のメソッドです。
        /// </summary>
        public static Method Enumerable_First = GetOneParamEnumerableMethod("First");

        /// <summary>
        /// <see cref="Enumerable.FirstOrDefault{T}(IEnumerable{T})"/>のメソッドです。
        /// </summary>
        public static Method Enumerable_FirstOrDefault = GetOneParamEnumerableMethod("FirstOrDefault");

        /// <summary>
        /// <see cref="IDictionary{T1, T2}.ContainsKey"/>のメソッドです。
        /// </summary>
        public static Method IDictionary_ContainsKey = FrameworkTypes.GenericIDictionary
            .GetMethod(Identifier.For("ContainsKey"), FrameworkTypes.GenericIDictionary.TemplateParameters[0]);

        /// <summary>
        /// <see cref="IDictionary{T1, T2}"/>のインデクサーで取得するメソッドです。
        /// </summary>
        public static Method IDictionary_GetIndexer = FrameworkTypes.GenericIDictionary
            .GetProperty(Identifier.For("Item"), FrameworkTypes.GenericIDictionary.TemplateParameters[0])
            .Getter;

        /// <summary>
        /// <see cref="IEnumerable{T}"/>のパラメーターのみ取る<see cref="Enumerable"/>のメソッドを取得します。
        /// </summary>
        private static Method GetOneParamEnumerableMethod(string methodName)
        {
            return FrameworkAssemblies.SystemCore.GetType(Identifier.For("System.Linq"), Identifier.For("Enumerable"))
                .GetMembersNamed(Identifier.For(methodName))
                .OfType<Method>()
                .First(method => method.Parameters.Count == 1 && method.Parameters[0].Type.IsAssignableToInstanceOf(FrameworkTypes.GenericIEnumerable));
        }
    }
}
