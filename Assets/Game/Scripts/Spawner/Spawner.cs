using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Game.Utils;

namespace Game.Spawner
{
    ///<summary>
    /// Class for instantiation objects whith some conditions
    ///</summary>
    public abstract class Spawner<T> : Singleton<Spawner<T>> where T : MonoBehaviour
    {
		[SerializeField] protected int minSpawnCount = 10;
		[SerializeField] protected Transform[] spawnPoints;
		[SerializeField] bool spawning;
		public bool Spawning 
		{
			get { return spawning; }
			set { spawning = value; }
		}

		public System.Action<T> onSpawn;

		List<T> spawnPool;

        ///<summary>
        ///<param name="spawnPoints">Array of available spawn points</param>
		///<param name="onSpawn">On Spawn callback<param>
        ///</summary>
        public void SetData(Transform[] spawnPoints, System.Action<T> onSpawn)
		{
			this.spawnPoints = spawnPoints;
			this.onSpawn = onSpawn;
		}

        ///<summary>
        /// Destroy all spawned objects
        ///</summary>
        public void Reset()
		{
			spawnPool.ForEach(x => Destroy(x.gameObject));
		}

		void Awake()
		{
			spawnPool = new List<T>();
		}

		void Update()
		{
			if (!Spawning)
			{
				return;
			}

			spawnPool = spawnPool.Where(x => x != null).ToList();

			while (spawnPool.Count < minSpawnCount)
			{
				T item = Factory.Factory<T>.Instance.GetRandom(spawnPoints.Random());
				
				item.transform.SetParent(transform);
				spawnPool.Add(item);

				if (onSpawn != null)
				{
					onSpawn(item);
				}
			}
		}
    }
}
