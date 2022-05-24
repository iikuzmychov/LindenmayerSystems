using System;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystems.Extensions;

internal static class IListExtensions
{
    internal static IEnumerable<TSource> TakeBackwards<TSource>(this IList<TSource> source, int startIndex)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (startIndex < 0)
            yield break;

        for (int i = startIndex; i >= 0; i--)
            yield return source[i];
    }
}
