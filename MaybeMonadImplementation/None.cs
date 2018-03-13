using System;
using Garaio.Framework;

namespace MaybeMonadImplementation
{
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
            if (type == typeof(None<>) 
                || type == typeof(None)
                || type == typeof(None<T>)) return true;

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

        public override bool IsSome { get { return false; } }
    }

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
}