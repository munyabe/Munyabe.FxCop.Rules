using System.Collections.Generic;
using System.Linq;
using Microsoft.FxCop.Sdk;
using Munyabe.FxCop.Util;

namespace Munyabe.FxCop.Maintainability
{
    /// <summary>
    /// readonly にできるフィールドを検出する解析ルールです。
    /// </summary>
    public class MarkPrivateFieldsAsReadOnly : BaseRule
    {
        private readonly HashSet<Field> _setFields = new HashSet<Field>();

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public MarkPrivateFieldsAsReadOnly()
            : base("MarkPrivateFieldsAsReadOnly")
        {
        }

        /// <inheritdoc />
        public override ProblemCollection Check(TypeNode type)
        {
            var fields = type.Members.OfType<Field>().Where(IsPossibleDefinition).ToArray();
            if (fields.Any())
            {
                Visit(type);

                fields
                    .Where(field => _setFields.Contains(field) == false)
                    .ForEach(field => Problems.Add(new Problem(GetResolution(field.Name.Name))));
            }

            return Problems;
        }

        /// <inheritdoc />
        public override void VisitAssignmentStatement(AssignmentStatement assignment)
        {
            var memberBinding = assignment.Target as MemberBinding;
            if (memberBinding == null)
            {
                return;
            }

            var field = memberBinding.BoundMember as Field;
            if (field == null)
            {
                return;
            }

            var thisObject = memberBinding.TargetObject as This;
            if (thisObject != null && thisObject.DeclaringMethod.IsInitializer() == false)
            {
                _setFields.Add(field);
            }
        }

        /// <summary>
        /// フィールドの定義から readonly を付与できる候補か判定します。
        /// </summary>
        private bool IsPossibleDefinition(Field field)
        {
            return field.IsLiteral == false && field.IsPrivate && field.IsInitOnly == false;
        }
    }
}
