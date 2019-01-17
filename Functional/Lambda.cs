using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Functional
{
    /// <summary>
    /// Helps the type interference system of C# (which only supports type interference for parameters) so 
    /// that implicitly typed local variables can be used - even with anonymous types
    /// </summary>
    public static class Lambda
    {
        public static Func<T> F<T>(Func<T> func)
        {
            return func;
        }

        public static Func<A, B> F<A,B>(Func<A,B> func)
        {
            return func;
        }

        public static Func<A, B, C> F<A, B, C>(Func<A, B, C> func)
        {
            return func;
        }
    }
}
