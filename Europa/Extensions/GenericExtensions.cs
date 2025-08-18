using System;
using System.Collections.Generic;
using System.Linq;

namespace Europa.Extensions
{
    public static class DictionaryExtensions
    {
        public static object GetValue<T>(this Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.IsNull())
            {
                return GetDefault<T>();
            }

            if (!dictionary.ContainsKey(key))
            {
                return GetDefault<T>();
            }

            return dictionary[key];
        }

        private static object GetDefault<T>()
        {
            switch (typeof(T).Name)
            {
                case "Int32":
                case "Int64": return 0;
                default: return string.Empty;
            }
        }
    }

    public static class ListExtensions
    {
        public static IEnumerable<TOutput> ConvertAll<T, TOutput>(this IEnumerable<T> enumerable, Converter<T, TOutput> converter)
        {
            if (enumerable.IsNull())
            {
                throw new ArgumentNullException("enumerable");
            }

            if (converter.IsNull())
            {
                throw new ArgumentNullException("converter");
            }

            int size = enumerable.Count();

            List<TOutput> list = new List<TOutput>(size);

            for (int i = 0; i < size; i++)
            {
                list.Add(converter(enumerable.ElementAt(i)));
            }

            return list;
        }

        public static bool HasDuplicates<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            if (enumerable.IsNull())
            {
                throw new ArgumentNullException("enumerable");
            }

            if (keySelector.IsNull())
            {
                throw new ArgumentNullException("keySelector");
            }

            return enumerable.GroupBy(keySelector).Where(i => i.Count() > 1).Count() > 0;
        }

        public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            if (source.IsNull())
            {
                throw new ArgumentNullException("source");
            }

            if (selector.IsNull())
            {
                throw new ArgumentNullException("selector");
            }

            return source.Aggregate(TimeSpan.Zero, (current, item) => current + selector(item));
        }
    }
}