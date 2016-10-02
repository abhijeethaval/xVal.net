using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XVal.Core
{
    internal static class PredicateExtensions
    {
        public static bool SatisfiedBy<TEntity>(this Predicate<TEntity> precondition, TEntity entity)
        {
            return precondition == null || precondition(entity);
        }
    }
}
