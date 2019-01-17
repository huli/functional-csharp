using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functional
{
    public class Continuation<T>
    {
        private readonly Maybe<T> _maybe;

        private Continuation(Maybe<T> toMaybe)
        {
            _maybe = toMaybe;
        }

        public static Continuation<T> Do(Func<T> action)
        {
            var r = action();
            return new Continuation<T>(r.ToMaybe());
        }

        public Maybe<TReturn> AndContinueWith<TReturn>(Func<T, TReturn> action)
        {
            if (_maybe.IsNone)
                return Maybe<TReturn>.None;

            return action(_maybe.Value).ToMaybe();
        }
    }
    public class Continuation
    {
        private bool _hasValidated = false;

        private Continuation(bool hasValidated)
        {
            _hasValidated = hasValidated;
        }

        public static Continuation Validate(Func<bool> validation)
        {
            var r = validation();
            return new Continuation(r);
        }

        public Maybe<T> AndContinueWith<T>(Func<T> action)
        {
            if (!_hasValidated)
                return Maybe<T>.None;

            return action().ToMaybe();
        }
    }
}
