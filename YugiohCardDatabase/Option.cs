using System;
using System.Collections.Generic;
using System.Text;

namespace YugiohCardDatabase
{
    public class Option<T> : IEquatable<Option<T>>
    {
        private readonly bool isValid;
        private readonly T item;

        internal Option(bool isValid, T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            this.isValid = isValid;
            this.item = item;
        }

        public Option<U> Map<U>(Func<T, U> f)
        {
            if (this.isValid)
            {
                return Option.Some(f(this.item));
            }
            else
            {
                return Option.None<U>();
            }
        }

        public T Unwrap()
        {
            if (this.isValid)
            {
                return this.item;
            }
            else
            {
                throw new InvalidOperationException("Cannot unwrap an invalid optional value.");
            }
        }

        public bool Equals(Option<T> other)
        {
            return this.isValid.Equals(other.isValid) && this.item.Equals(other.item);
        }

        public override bool Equals(object obj)
        {
            return obj is Option<T> other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.isValid.GetHashCode() ^ this.item.GetHashCode();
        }
    }

    public static class Option
    {
        public static Option<T> Some<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            return new Option<T>(true, item);
        }

        public static Option<T> None<T>()
        {
            return new Option<T>(false, default);
        }

        public static Option<T> FromNullableStruct<T>(T? item)
            where T : struct
        {
            if (item == null)
            {
                return None<T>();
            }
            else
            {
                return Some(item.Value);
            }
        }
    }
}
