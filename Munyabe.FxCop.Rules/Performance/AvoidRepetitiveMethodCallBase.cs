using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Performance
{
    /// <summary>
    /// 同じ値に対して指定のメソッドを繰り返し呼び出していることを検出する解析ルールの基底クラスです。
    /// </summary>
    public abstract class AvoidRepetitiveMethodCallBase : BaseRule
    {
        /// <summary>
        /// <see cref="Enumerable.Any{T}(IEnumerable{T})"/>が呼び出されたノードです。
        /// </summary>
        private HashSet<Node> _calledNodes = new HashSet<Node>();

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="name">ルール名</param>
        protected AvoidRepetitiveMethodCallBase(string name)
            : base(name)
        {
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
            base.VisitMethodCall(call);

            if (IsFirstTargetMethodCall(call))
            {
                var node = call.GetCalledNode();
                if (node != null)
                {
                    _calledNodes.Add(node);
                }
            }
            else if (_calledNodes.Count != 0 && IsSecondTargetMethodCall(call))
            {
                var node = call.GetCalledNode();
                if (_calledNodes.Contains(node))
                {
                    Problems.Add(CreateProblem(call));
                }
            }
        }

        /// <inheritdoc />
        public override void VisitAssignmentStatement(AssignmentStatement assignment)
        {
            base.VisitAssignmentStatement(assignment);
            _calledNodes.Remove(assignment.Target);
        }

        /// <summary>
        /// 1つめの検出対象のメソッド呼び出しかどうかを判定します。
        /// </summary>
        /// <param name="call">メソッド呼び出し</param>
        /// <returns>検出対象のメソッド呼び出しの場合は<see langword="true"/></returns>
        protected abstract bool IsFirstTargetMethodCall(MethodCall call);

        /// <summary>
        /// 2つめの検出対象のメソッド呼び出しかどうかを判定します。
        /// </summary>
        /// <param name="call">メソッド呼び出し</param>
        /// <returns>検出対象のメソッド呼び出しの場合は<see langword="true"/></returns>
        protected abstract bool IsSecondTargetMethodCall(MethodCall call);
    }
}
