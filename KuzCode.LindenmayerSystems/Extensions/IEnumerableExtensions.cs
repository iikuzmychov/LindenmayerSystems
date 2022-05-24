using System;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystems.Extensions;

internal static class IEnumerableExtensions
{
    internal static TSource[] TakeToArray<TSource>(this IEnumerable<TSource> source, int count)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var array            = new TSource[count];
        var sourceEnumerator = source.GetEnumerator();

        for (int i = 0; i < count && sourceEnumerator.MoveNext(); i++)
            array[i] = sourceEnumerator.Current;

        return array;
    }
}
