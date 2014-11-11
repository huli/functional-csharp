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

    public abstract class Maybe
    {
        public static None None
        {
            get { return new None(); }
        }

        public abstract T OrElse<T>(Func<T> alternative);
    }

    public abstract class Maybe<T>
    {
        public static Maybe<T> Some
        {
            get { return new Some<T>(default(T)); }
        }

        public T Value { get; protected set; }

        public abstract T OrElse(Func<T> alternative);
        public abstract Maybe<T> OrElse(Func<Maybe<T>> alternative);
    }

    public interface INone
    { }

    public class None : Maybe, INone
    {
        protected bool Equals(None other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj is INone) return true;

            return false;
        }

        public override string ToString()
        {
            return "None";
        }

        public override int GetHashCode()
        {
            return None.GetHashCode();
        }

        public override T OrElse<T>(Func<T> alternative)
        {
            return alternative();
        }
    }


    public class None<T> : Maybe<T>, IEquatable<T>, INone
    {
        public bool Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var type = other.GetType();
            if (type == typeof(None<>) || type == typeof(None)) return true;

            return false;
        }

        public override string ToString()
        {
            return "None";
        }

        protected bool Equals(None<T> other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return true;
            if (ReferenceEquals(this, obj)) return true;

            var type = obj.GetType();
            if (type == typeof(None<>) || type == typeof(None)) return true;

            return false;
        }

        public override int GetHashCode()
        {
            return Maybe.None.GetHashCode();
        }

        public override T OrElse(Func<T> alternative)
        {
            return alternative();
        }

        public override Maybe<T> OrElse(Func<Maybe<T>> alternative)
        {
            return alternative();
        }
    }

    public class Some<T> : Maybe<T>
    {
        protected bool Equals(Some<T> other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != this.GetType()) return false;

            return Equals(Value, ((Some<T>)obj).Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public Some(T value)
        {
            Value = value;
        }

        public override T OrElse(Func<T> alternative)
        {
            return Value;
        }

        public override Maybe<T> OrElse(Func<Maybe<T>> alternative)
        {
            return this;
        }
    }
}
