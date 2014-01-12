using System;
using Microsoft.FxCop.Sdk;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// <see cref="Enum.GetHashCode"/>の呼び出しを検出する解析ルールです。
    /// </summary>
    public class AvoidEnumGetHashCode : BaseRule
    {
        private static Method _getHashCodeMethod;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public AvoidEnumGetHashCode()
            : base("AvoidEnumGetHashCode")
        {
        }

        /// <inheritdoc />
        public override void BeforeAnalysis()
        {
            _getHashCodeMethod = FrameworkTypes.Object.GetMethod(Identifier.For("GetHashCode"));
        }

        /// <inheritdoc />
        public override void AfterAnalysis()
        {
            _getHashCodeMethod = null;
        }

        /// <inheritdoc />
        public override ProblemCollection Check(Member member)
        {
            if (member is Method)
            {
                Visit(member);
            }

            return Problems;
        }

        /// <inheritdoc />
        public override void VisitMethodCall(MethodCall call)
        {
            if (IsEnumGetHashCodeCall(call))
            {
                Problems.Add(CreateProblem(call));
            }
        }

        /// <summary>
        /// <see cref="Enum.GetHashCode"/>の呼び出しかどうかを判定します。
        /// </summary>
        private static bool IsEnumGetHashCodeCall(MethodCall call)
        {
            var member = call.Callee as MemberBinding;
            if (member == null)
            {
                return false;
            }

            if (member.TargetObject == null || member.TargetObject.Type != FrameworkTypes.Enum)
            {
                return false;
            }

            var method = member.BoundMember as Method;
            if (method == null)
            {
                return false;
            }

            return method == _getHashCodeMethod;
        }
    }
}
