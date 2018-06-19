using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public static class CollectionsHelper
    {
        ///<summary>
        ///Extension method.
        ///Return random element of IList<>
        ///</summary>
		public static T Random<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
