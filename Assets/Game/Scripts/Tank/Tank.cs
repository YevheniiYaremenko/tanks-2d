﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game
{
    public abstract class Tank : DamagingObject, IDamaging, IMovable
    {
        [SerializeField] Image weaponBar;

		[Header("Movement")]
		[SerializeField] float movementSpeed = 1;
		[SerializeField] float inverseMoveMult = .5f;
		[SerializeField] float rotationSpeed = 90;
        float sceneHalfSize;
        bool outBounds = false;

		[Header("Weapon")]
		[SerializeField] Transform[] weaponBases;
        [SerializeField] Weapon[] availableWeapons;
		int currentWeaponId = 0;
		Weapon[] installedWeapons;

		protected override void Awake()
		{
            base.Awake();
            installedWeapons = new Weapon[weaponBases.Length];
		}

        void Update()
		{
			weaponBar.transform.parent.gameObject.SetActive(installedWeapons[0] != null && installedWeapons[0].ReloadingProgress < 1 && installedWeapons[0].ReloadingDuration >= 1);
			if (installedWeapons[0] != null)
			{
				weaponBar.fillAmount = installedWeapons[0].ReloadingProgress;
			}
		}

		public void SetData(float sceneSize)
		{
            currentWeaponId = 0;
			LoadWeapon();

            sceneHalfSize = sceneSize / 2f;
		}

        #region Weapon

        public void NextWeapon()
        {
            currentWeaponId = currentWeaponId < availableWeapons.Length - 1
                ? currentWeaponId + 1
                : 0;
            LoadWeapon();
        }

        public void PreviousWeapon()
        {
            currentWeaponId = currentWeaponId > 0
                ? currentWeaponId - 1
                : availableWeapons.Length - 1;
            LoadWeapon();
        }

        public void LoadWeapon()
        {
			for (int i = 0; i < installedWeapons.Length; i++)
			{
                if (installedWeapons[i] != null)
                {
                    DestroyImmediate(installedWeapons[i].gameObject);
                }
                installedWeapons[i] = Instantiate(availableWeapons[currentWeaponId],weaponBases[i].position, weaponBases[i].rotation, weaponBases[i]);
			}
        }

		public void Shoot()
		{
			foreach(var weapon in installedWeapons)
			{
                if (weapon != null)
                {
                    weapon.Shoot();
                }
			}
		}

		public void ShootAutomatically()
		{
            foreach (var weapon in installedWeapons)
            {
                if (weapon != null && weapon.Automatic)
                {
                    weapon.Shoot();
                }
            }
		}

		#endregion

		#region IMovable

		public void Move(float direction)
		{
			var newPosition = transform.position + (transform.up * direction).normalized * movementSpeed * Mathf.Abs(direction) * Time.deltaTime * (direction >= 0 ? 1 : inverseMoveMult);
            if (outBounds && IsOutOfBounds(newPosition))
			{
				return;
			}
			outBounds = IsOutOfBounds(newPosition);
			transform.position = new Vector3(
                Mathf.Clamp(newPosition.x, -sceneHalfSize, sceneHalfSize),
                Mathf.Clamp(newPosition.y, -sceneHalfSize, sceneHalfSize)
			);
		}

		bool IsOutOfBounds(Vector3 position)
		{
			return Mathf.Abs(position.x) > sceneHalfSize || Mathf.Abs(position.y) > sceneHalfSize;
		}

		public void Rotate(float direction)
		{
            transform.Rotate( -Vector3.forward * rotationSpeed * direction * Time.deltaTime);
		}

        #endregion
    }
}
