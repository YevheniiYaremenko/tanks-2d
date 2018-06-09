using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.View
{
    public class LevelView : MonoBehaviour
    {
		[SerializeField] Transform[] playerSpawnPoints;
		[SerializeField] Transform[] enemySpawnPoints;

        public Transform[] PlayerSpawnPoints { get { return playerSpawnPoints; } }
        public Transform[] EnemySpawnPoints { get { return enemySpawnPoints; } }
    }
}
