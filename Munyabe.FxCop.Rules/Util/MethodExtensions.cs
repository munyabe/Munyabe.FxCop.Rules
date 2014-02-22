using System;
using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Util
{
    /// <summary>
    /// <see cref="Method"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class MethodExtensions
    {
        /// <summary>
        /// 指定のメソッドの具象メソッドかどうかを判定します。
        /// </summary>
        /// <param name="method">判定するメソッド</param>
        /// <param name="baseMethod">仮装メソッドまたはインターフェースのメソッド定義</param>
        /// <returns>指定のメソッドの具象メソッドの場合は<see langword="true"/></returns>
        public static bool IsInherit(this Method method, Method baseMethod)
        {
            Guard.ArgumentNotNull(method, "method");
            Guard.ArgumentNotNull(baseMethod, "baseMethod");

            Method targetMethod;
            Method targetBaseMethod;

            if (method.Template != null)
            {
                targetMethod = method.Template;
                targetBaseMethod = baseMethod.Template ?? baseMethod;
            }
            else
            {
                targetMethod = method;
                targetBaseMethod = baseMethod;
            }

            return targetMethod.DeclaringType.IsAssignableTo(targetBaseMethod.DeclaringType) &&
                targetMethod.Name.Name == targetBaseMethod.Name.Name &&
                ReturnTypeMatchStructurally(targetMethod, targetBaseMethod.ReturnType) &&
                targetMethod.ParametersMatchStructurally(targetBaseMethod.Parameters);
        }

        /// <summary>
        /// コンストラクターがどうかを判定します。
        /// </summary>
        /// <param name="method">判定するメソッド</param>
        /// <returns>コンストラクターの場合は<see langword="true"/></returns>
        public static bool IsInitializer(this Method method)
        {
            Guard.ArgumentNotNull(method, "method");

            return method is InstanceInitializer || method is StaticInitializer;
        }

        /// <summary>
        /// 自動生成されたプロパティのメソッドかどうかを判定します。
        /// </summary>
        /// <param name="method">判定するメソッド</param>
        /// <returns>プロパティのメソッドの場合は<see langword="true"/></returns>
        public static bool IsPropertyAccessor(this Method method)
        {
            Guard.ArgumentNotNull(method, "method");

            var name = method.Name.Name;
            return name.StartsWith("get_", StringComparison.Ordinal) || name.StartsWith("set_", StringComparison.Ordinal);
        }

        /// <summary>
        /// 戻り値の型の構造が一致するかどうかを判定します。
        /// </summary>
        /// <param name="method">判定するメソッド</param>
        /// <param name="type">戻り値の型</param>
        /// <returns>戻り値の型の構造が一致する場合は<see langword="true"/></returns>
        private static bool ReturnTypeMatchStructurally(Method method, TypeNode type)
        {
            Guard.ArgumentNotNull(method, "method");

            var typeParameter = method.ReturnType as TypeParameter;
            return typeParameter != null ? typeParameter.IsStructurallyEquivalentTo(type) : method.ReturnType == type;
        }
    }
}
