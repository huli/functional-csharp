using System;
using MaybeMonadImplementation;

namespace Garaio.Framework
{
    public abstract class Maybe<T>
    {
        public static Maybe<T> Some
        {
            get { return new Some<T>(default(T)); }
        }

        public static Maybe<T> None
        {
            get { return new None<T>(); }
        }

        public T Value { get; protected set; }
        public abstract T OrElse(Func<T> alternative);
        public abstract Maybe<T> OrElse(Func<Maybe<T>> alternative);
        public abstract bool IsSome { get; }
    }

    public abstract class Maybe
    {
        public static Maybe None
        {
            get { return new None(); }
        }
        public abstract T OrElse<T>(Func<T> alternative);
    }
}