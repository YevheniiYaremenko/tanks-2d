using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Tank : MonoBehaviour, IDamageHandler, IMovable
    {
		[SerializeField] float maxHealth = 100;
		[Range(0f, 1f)] [SerializeField] float defence = 0.5f;
		[SerializeField] float movementSpeed = 1;
		[SerializeField] float rotationSpeed = 90;
		[SerializeField] Transform[] weaponBases;

		int currentWeaponId = 0;
		Weapon weapon;
		Weapon[] weapons;

		void Awake()
		{
			Health = maxHealth;
		}

		public void SetData(Weapon[] weapons)
		{
			this.weapons = weapons;
		}

        #region Weapon

        public void NextWeapon()
        {
            currentWeaponId = currentWeaponId < weapons.Length - 1
                ? currentWeaponId + 1
                : 0;
            LoadWeapon();
        }

        public void PreviousWeapon()
        {
            currentWeaponId = currentWeaponId > 0
                ? currentWeaponId - 1
                : weapons.Length - 1;
            LoadWeapon();
        }

        public void LoadWeapon()
        {
            throw new System.NotImplementedException();
        }

		public void Shoot()
		{
			if (weapon != null)
			{
				weapon.Shoot();
			}
		}

		#endregion

		#region IMovable

		public void Move(float direction)
		{
			transform.position += (transform.up * direction).normalized * movementSpeed * Mathf.Abs(direction) * Time.deltaTime;
			//TODO: check moving bounds
		}

		public void Rotate(float direction)
		{
            transform.Rotate( -Vector3.forward * rotationSpeed * direction * Time.deltaTime);
		}

        #endregion

        #region IDamageHandler

        public float Health { get; private set; }
		public event System.Action onDeath;

		public void DealDamage(float damage)
		{
			Health = Mathf.Max(Health - damage * (1 - defence));
			if (Health == 0)
			{
				Death();
			}
		}

		public void Death()
		{
			if (onDeath != null)
			{
				onDeath.Invoke();
			}
		}

		#endregion
    }
}
