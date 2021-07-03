using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystem
{
    internal static class ListExtensions
    {
        public static List<T> Clone<T>(this List<T> source) where T : ICloneable
        {
            return source.Select(item => (T)item.Clone()).ToList();
        }
    }
}
