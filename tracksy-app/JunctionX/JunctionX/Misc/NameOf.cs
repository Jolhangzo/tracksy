using System;
using System.Linq.Expressions;

namespace JunctionX.Misc
{
    public class NameOf<T>
    {
        public static string Property<TProp>(Expression<Func<T, TProp>> expression)
        {
            if (!(expression.Body is MemberExpression body))
                throw new ArgumentException("'expression' should be a member expression");
            return body.Member.Name;
        }
    }
}
