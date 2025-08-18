using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Europa.Data.Dialect
{
    public class DecomposeGenerator : BaseHqlGeneratorForMethod
    {
        public DecomposeGenerator()
        {
            SupportedMethods = new[] { ReflectionHelper.GetMethodDefinition(() => DecomposeExtension.Decompose(0, 0)) };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject,
            ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            IEnumerable<HqlExpression> args = arguments.Select(a => visitor.Visit(a))
                .Cast<HqlExpression>();

            return treeBuilder.MethodCall("FN_DECOMPOSE", args);
        }
    }

    public class AllLinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public AllLinqToHqlGeneratorsRegistry()
        {
            //RegisterGenerator()
            this.Merge(new DecomposeGenerator());
        }
    }
}
