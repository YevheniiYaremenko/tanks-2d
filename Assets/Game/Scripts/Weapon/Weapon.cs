using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Weapon : MonoBehaviour
    {
		[SerializeField] protected float damage;

		public abstract void Shoot();
    }
}
