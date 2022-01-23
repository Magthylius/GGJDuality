using System.Collections.Generic;

namespace Magthylius
{
    public static class ListEx
    {
        /// <summary>Gets a random element in the list.</summary>
        public static T Random<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>Gets a random index based on elements in the list.</summary>
        public static int RandomIndex<T>(this List<T> list)
        {
            return UnityEngine.Random.Range(0, list.Count);
        }

        /// <summary>Gets a reversed copy of the current list.</summary>
        public static List<T> Reversed<T>(this List<T> list)
        {
            List<T> copy = new List<T>(list);
            copy.Reverse();
            return copy;
        }

        //! TODO: Make efficient
        /// <summary>Gets a reversed sorted copy of the current list.</summary>
        public static List<T> ReverseSort<T>(this List<T> list, Comparer<T> comparer)
        {
            List<T> copy = new List<T>(list);
            copy.Sort(comparer);
            copy.Reverse();
            return copy;
        }
    }
}
