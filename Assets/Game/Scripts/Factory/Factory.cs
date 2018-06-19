using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Game.Utils;

namespace Game.Factory
{
	///<summary>
	/// Class for instantiation objects from factory pool
	///</summary>
    public abstract class Factory<T> : Singleton<Factory<T>> where T : MonoBehaviour
    {
        [SerializeField] protected T[] pool;

        ///<summary>
        /// Return all types created by the factory
        ///</summary>
        public System.Type[] GetTypes()
		{
			return pool.Select(x => x.GetType()).Distinct().ToArray();
		}

        ///<summary>
        ///Instantiate and return item with type 
		///<param name="type">Type of the instantiated object</param>
        ///</summary>
        public virtual X GetItem<X>(System.Type type) where X : T
		{
			var items = pool.Where(x => x.GetType() == type).Select(x => x as X).ToArray();
			return items.Count() > 0 ? Instantiate(items.Random()) : null;
		}

        ///<summary>
        ///Instantiate and return random item from the factory pool 
        ///<param name="spawnPoint">Start position and rotation reference</param>
        ///</summary>
        public virtual T GetRandom(Transform spawnPoint = null)
		{
            if (spawnPoint != null)
			{
                return Instantiate(pool.Random(), spawnPoint.transform.position, spawnPoint.transform.rotation);
			}
			else
			{
                return Instantiate(pool.Random());
			}
		}
    }
}
