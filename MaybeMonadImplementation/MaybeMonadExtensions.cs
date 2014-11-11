using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaybeMonadImplementation
{
    public static class MonadExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T obj)
        {
            if(CompareWithNull(obj))
                return new None<T>();

            return new Some<T>(obj);
        }

        public static Maybe<TReturn> With<T, TReturn>(this T obj, Func<T, TReturn> func)
            where T : class
        {

            if (CompareWithNull(obj))
                return new None<TReturn>();

            var result = func(obj);
            if (CompareWithNull(result))
                return new None<TReturn>();

            return new Some<TReturn>(result);
        }

        public static Maybe<B> Bind<A, B>(this Maybe<A> m, Func<A, Maybe<B>> func)
        {
            var someA = m as Some<A>;
            return someA == null ? new None<B>() : func(someA.Value);
        }

        public static Maybe<C> SelectMany<A, B, C>(this Maybe<A> a, Func<A, Maybe<B>> func, Func<A, B, C> select)
        {
            // correct: 
            // return select(a.Value, a.Bind(func).Value).ToMaybe();

            // more pure:
            return a.Bind(aval =>
                func(aval).Bind(bval =>
                select(aval, bval).ToMaybe()));
        }


        public static Maybe<IEnumerable<TReturn>> With<T, TReturn>(this IEnumerable<T> list, Func<T, TReturn> selector)
        {
            if (CompareWithNull(list))
                return new None<IEnumerable<TReturn>>();

            return list.Filter().Select(selector).Filter().ToMaybe();
        }

        public static Maybe<IEnumerable<TReturn>> With<T, TReturn>(this Maybe<IEnumerable<T>> list, Func<T, TReturn> selector)
        {
            if (Equals(list, new None<IEnumerable<T>>()))
                return new None<IEnumerable<TReturn>>();

            return list.Value.Select(selector).Filter().ToMaybe();

        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> list)
        {
            return list.Where(element => !CompareWithNull(element));
        }

        private static bool CompareWithNull<T>(T element)
        {
            return element == null;
        }
    }
}
