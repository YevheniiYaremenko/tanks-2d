using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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
		[SerializeField] protected Transform spawnCenter;
		[SerializeField] protected float spawnRadius;
		[SerializeField] bool spawning;
		public bool Spawning 
		{
			get { return spawning; }
			set { spawning = value; }
		}

		public System.Action<T> onSpawn;

		List<T> spawnPool;

		public void SetData(float spawnRadius, System.Action<T> onSpawn)
		{
			this.spawnRadius = spawnRadius;
			this.onSpawn = onSpawn;
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
				var angle = Random.Range(0, Mathf.PI);
				var direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
				item.transform.position = spawnCenter.position + spawnRadius * direction;
				item.transform.up = -direction;
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
