using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTools
{
    /// <summary>
    /// Definition for currying and partial function application for functions
    /// up to 4 params (and 2 partial applied params).
    /// HINT: The patterns should be obvious at this point, define more if needed.
    /// </summary>
    public static class FunctionalExtensions
    {
        #region Partial Application
        public static Func<TR> ApplyPartially<T1, TR>(this Func<T1, TR> func, T1 a)
        {
            return () => func(a);
        }

        public static Func<T2, TR> ApplyPartially<T1, T2, TR>(this Func<T1, T2, TR> func, T1 a)
        {
            return  b => func(a, b);
        }

        public static Func<T3, TR> ApplyPartially<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func, T1 a, T2 b)
        {
            return c => func(a, b, c);
        }

        public static Func<T2, T3, TR> ApplyPartially<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func, T1 a)
        {
            return (b, c) => func(a, b, c);
        }

        public static Func<T2, T3, T4, TR> ApplyPartially<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func, T1 a)
        {
            return (b, c, d) => func(a, b, c, d);
        }

        public static Func<T3, T4, TR> ApplyPartially<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func, T1 a, T2 b)
        {
            return (c, d) => func(a, b, c, d);
        }

        #endregion

        #region Schoenfinkeling
        public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(this Func<T1, T2, TR> func)
        {
            return a => b => func(a, b);
        }

        public static Func<T1, Func<T2, Func<T3, TR>>> Curry<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func)
        {
            return a => b => c => func(a, b, c);
        }

        public static Func<T1, Func<T2, Func<T3, Func<T4, TR>>>> Curry<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func)
        {
            return a => b => c => d => func(a, b, c, d);
        }
        #endregion  
    }
}
