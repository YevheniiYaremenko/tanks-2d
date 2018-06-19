using UnityEngine;

namespace Game.View
{
    public class LevelView : MonoBehaviour
    {
		[SerializeField] Transform[] playerSpawnPoints;
		[SerializeField] Transform[] enemySpawnPoints;

        public Transform[] PlayerSpawnPoints => playerSpawnPoints;
        public Transform[] EnemySpawnPoints => enemySpawnPoints;
    }
}
