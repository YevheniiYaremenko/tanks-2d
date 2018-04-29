using UnityEngine;
using Game;

namespace Game.Factory
{
    public class TankFactory : MonoBehaviour, IFactory<Tank>
    {
        static TankFactory instance;
        public static TankFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<TankFactory>();
                }
                return instance;
            }
        }
        [SerializeField] Tank[] tankPool;

        public Tank GetRandom()
        {
            return Instantiate(tankPool[Random.Range(0, tankPool.Length)]);
        }
    }
}
