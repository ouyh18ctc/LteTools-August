using System;
using System.Collections.Generic;
using System.Linq;

namespace Lte.Domain.TypeDefs
{
    public class CommonEqualityComparer<T, TV> : IEqualityComparer<T>
    {
        private Func<T, TV> keySelector;

        public CommonEqualityComparer(Func<T, TV> keySelector)
        {
            this.keySelector = keySelector;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<TV>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<TV>.Default.GetHashCode(keySelector(obj));
        }
    }

    public static class DistinctExtensions
    {
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector));
        }
    }
}
