using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// 同じ値に対して最初の要素を繰り返し列挙していることを検出する解析ルールです。
    /// </summary>
    public class AvoidRepetitiveFirstEnumeration : BaseRule
    {
        /// <summary>
        /// <see cref="Enumerable.Any{T}(IEnumerable{T})"/>が呼び出されたノードです。
        /// </summary>
        private HashSet<Node> _calledAnyNodes = new HashSet<Node>();

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public AvoidRepetitiveFirstEnumeration()
            : base("AvoidRepetitiveFirstEnumeration")
        {
        }

        /// <inheritdoc />
        public override ProblemCollection Check(Member member)
        {
            if (member is Method)
            {
                Visit(member);
                _calledAnyNodes.Clear();
            }

            return Problems;
        }

        /// <inheritdoc />
        public override void VisitMethodCall(MethodCall call)
        {
            base.VisitMethodCall(call);

            if (call.IsCall(SystemMembers.Enumerable_Any))
            {
                var node = GetCalledNode(call);
                if (node != null)
                {
                    _calledAnyNodes.Add(node);
                }
            }

            if (_calledAnyNodes.Any() &&
                (call.IsCall(SystemMembers.Enumerable_First) || call.IsCall(SystemMembers.Enumerable_FirstOrDefault)))
            {
                var node = GetCalledNode(call);
                if (_calledAnyNodes.Contains(node))
                {
                    Problems.Add(CreateProblem(call));
                }
            }
        }

        /// <inheritdoc />
        public override void VisitAssignmentStatement(AssignmentStatement assignment)
        {
            base.VisitAssignmentStatement(assignment);
            _calledAnyNodes.Remove(assignment.Target);
        }

        /// <summary>
        /// メソッドが呼び出されたノードを取得します。
        /// </summary>
        private static Node GetCalledNode(MethodCall call)
        {
            var operand = call.Operands[0];
            if (operand is Local)
            {
                return operand;
            }
            else
            {
                // MEMO : フィールド
                var binding = operand as MemberBinding;
                if (binding != null)
                {
                    return binding.BoundMember;
                }

                // MEMO : プロパティ
                var propertyCall = operand as MethodCall;
                if (propertyCall != null)
                {
                    var propertyBinding = propertyCall.Callee as MemberBinding;
                    if (propertyBinding != null)
                    {
                        var accessor = propertyBinding.BoundMember as Method;
                        if (accessor != null && accessor.IsPropertyAccessor())
                        {
                            return accessor;
                        }
                    }
                }
            }

            return null;
        }
    }
}
