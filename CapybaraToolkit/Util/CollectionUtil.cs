using System.Collections.Generic;
using System;
using System.Linq;

namespace CapybaraToolkit.Util
{
    public static class CollectionUtil
    {
        public static IEnumerable<(int index, T value)> Enumerate<T>(this IEnumerable<T> collection) => collection.Select((v, i) => (i, v));
    }
}
