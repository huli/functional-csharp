using System;

namespace MaybeMonadImplementation
{
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