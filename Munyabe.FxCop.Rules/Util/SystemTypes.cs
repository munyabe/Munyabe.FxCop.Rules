using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="System"/>名前空間の型です。
    /// </summary>
    public static class SystemTypes
    {
        /// <summary>
        /// <see cref="System.EventHandler"/>のデリゲートです。
        /// </summary>
        public static TypeNode EventHandler = FrameworkAssemblies.Mscorlib.GetType(Identifier.For("System"), Identifier.For("EventHandler"));

        /// <summary>
        /// <see cref="System.EventHandler{T}"/>のデリゲートです。
        /// </summary>
        public static TypeNode GenericEventHandler = FrameworkAssemblies.Mscorlib.GetType(Identifier.For("System"), Identifier.For("EventHandler`1"));

        /// <summary>
        /// <see cref="System.IAsyncResult"/>のインターフェースです。
        /// </summary>
        public static TypeNode IAsyncResult = FrameworkAssemblies.Mscorlib.GetType(Identifier.For("System"), Identifier.For("IAsyncResult"));
    }
}
