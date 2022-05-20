using System;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystems.Extensions;

internal static class IListExtensions
{
    internal static IEnumerable<T> TakeBackwards<T>(this IList<T> source, int startIndex)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (startIndex < 0)
            yield break;

        for (int i = startIndex; i >= 0; i--)
            yield return source[i];
    }
}
