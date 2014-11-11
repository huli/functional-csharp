using System;

namespace MaybeMonadImplementation
{
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

    public abstract class Maybe
    {
        public static None None
        {
            get { return new None(); }
        }

        public abstract T OrElse<T>(Func<T> alternative);
    }
}