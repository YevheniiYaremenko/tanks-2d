using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.View
{
    public class LevelView : MonoBehaviour
    {
		[SerializeField] Transform[] playerSpawnPoints;
		[SerializeField] Transform[] enemySpawnPoints;

        public Transform[] PlayerSpawnPoints { get { return playerSpawnPoints; } }
        public Transform[] EnemySpawnPoints { get { return enemySpawnPoints; } }

		public void Show()
		{
			gameObject.SetActive(true);
		}

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        [ContextMenu("Bake Nav Mesh Map")]
        [ExecuteInEditMode]
        public void BakeNavMesh()
        {
            throw new System.NotImplementedException();
        }
    }
}
