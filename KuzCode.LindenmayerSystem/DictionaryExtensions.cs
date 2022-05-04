using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystem
{
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Adapted from <see href="https://stackoverflow.com/questions/56692/random-weighted-choice#answer-56735">this</see> question from StackOverflow
        /// </summary>
        public static TKey GetRandomKeyByWeight<TKey>(this Dictionary<TKey, int> dictionary, int seed)
        {
            if (dictionary.Values.Any(value => value < 0))
                throw new ArgumentException("Weight can not be less than zero");

            int totalWeight  = dictionary.Sum(pair => pair.Value);
            int randomNumber = new Random(seed).Next(0, totalWeight);
            TKey selectedKey = default(TKey);

            foreach (var item in dictionary
                .Select(pair => (Key: pair.Key, Weight: pair.Value)))
            {
                if (randomNumber < item.Weight)
                {
                    selectedKey = item.Key;
                    break;
                }

                randomNumber -= item.Weight;
            }

            return selectedKey;
        }
    }
}
