using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace YugiohCardDatabase
{
    public class Option<T> : IEquatable<Option<T>>
    {
        private readonly bool isValid;
        private readonly T item;

        internal Option(bool isValid, T item)
        {
            if (isValid && item == null)
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

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
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

        public T UnwrapOr(T defaultValue)
        {
            if (this.isValid)
            {
                return this.item;
            }
            else
            {
                return defaultValue;
            }
        }

        public void MayAct(Action<T> action)
        {
            if (this.isValid)
            {
                action(this.item);
            }
        }

        public bool Equals(Option<T> other)
        {
            if (this.isValid && other.isValid)
            {
                return this.item?.Equals(other.item) ?? other.item == null;
            }
            else
            {
                return this.isValid == other.isValid;
            }
        }
    }

    public static class Option
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
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

        public static Option<T> FromNullableClass<T>(T? item) where T : class
        {
            if (item == null)
            {
                return None<T>();
            }
            else
            {
                return Some(item);
            }
        }
    }
}
