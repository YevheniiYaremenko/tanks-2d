using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Game.Utils;

namespace Game.Spawner
{
    public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
    {
		static Spawner<T> instance;
		public static Spawner<T> Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<Spawner<T>>();
				}
				return instance;
			}
		}
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

		public void SetData(Transform[] spawnPoints, System.Action<T> onSpawn)
		{
			this.spawnPoints = spawnPoints;
			this.onSpawn = onSpawn;
		}

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
				T item = Factory.Factory<T>.Instance.GetRandom();

				var spawnPoint = spawnPoints.Random();
				item.transform.position = spawnPoint.position;
				item.transform.up = spawnPoint.eulerAngles;
				
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
