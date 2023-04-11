using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary> Returns random element from collection </summary>
        /// <param name="collection"> Collection to choose from </param>
        /// <param name="random"> Optional seeded random number generator (otherwise uses UnityEngine.Random) </param>
        /// <returns> Random collection element </returns>
        public static T GetRandom<T>(this IEnumerable<T> collection, Random random = null)
        {
            return GetScrambled(collection, random).First();
        }

        /// <summary> Returns scrambled copy of collection </summary>
        /// <param name="collection"> Collection to copy from </param>
        /// <param name="random"> Optional seeded random number generator (otherwise uses UnityEngine.Random) </param>
        /// <returns> Scrambled copy of collection </returns>
        public static IEnumerable<T> GetScrambled<T>(this IEnumerable<T> collection, Random random = null)
        {
            return random != null ? collection.OrderBy(_ => random.Next()) : collection.OrderBy(_ => UnityEngine.Random.value);
        }

        /// <summary> Calls action on every element of the collection </summary>
        /// <param name="collection"> Collection to iterate </param>
        /// <param name="action"> Action to call </param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T element in collection)
            {
                action.Invoke(element);
            }
        }
    }
}