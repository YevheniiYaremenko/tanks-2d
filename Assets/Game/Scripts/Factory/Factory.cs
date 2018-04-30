using UnityEngine;
using System.Linq;

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

		public virtual X GetItem<X>() where X : T
		{
			return pool.FirstOrDefault(x => x is X) as X;
		}

		public virtual T GetRandom()
		{
            return Instantiate(pool[Random.Range(0, pool.Length)]);
		}
    }
}
