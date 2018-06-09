using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Game.Utils;

namespace Game.Factory
{
    public abstract class Factory<T> : MonoBehaviour where T : MonoBehaviour
    {
		static Factory<T> instance;
		public static Factory<T> Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<Factory<T>>();
				}
				return instance;
			}
		}

        [SerializeField] protected T[] pool;

		public System.Type[] GetTypes()
		{
			return pool.Select(x => x.GetType()).Distinct().ToArray();
		}

		public virtual X GetItem<X>(System.Type type) where X : T
		{
			var items = pool.Where(x => x.GetType() == type).Select(x => x as X).ToArray();
			return items.Count() > 0 ? Instantiate(items.Random()) : null;
		}

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
