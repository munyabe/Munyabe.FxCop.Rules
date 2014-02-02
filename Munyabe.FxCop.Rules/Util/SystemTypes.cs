using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="System"/>名前空間の型です。
    /// </summary>
    public static class SystemTypes
    {
        /// <summary>
        /// <see cref="System.EventHandler"/>のクラスです。
        /// </summary>
        public static TypeNode EventHandler = FrameworkAssemblies.Mscorlib.GetType(Identifier.For("System"), Identifier.For("EventHandler"));

        /// <summary>
        /// <see cref="System.EventHandler{T}"/>のクラスです。
        /// </summary>
        public static TypeNode GenericEventHandler = FrameworkAssemblies.Mscorlib.GetType(Identifier.For("System"), Identifier.For("EventHandler`1"));
    }
}
