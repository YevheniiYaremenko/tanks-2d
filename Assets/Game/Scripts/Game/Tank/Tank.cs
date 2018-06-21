using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.Mathematics;

namespace Game
{
	[RequireComponent(typeof(Rigidbody2D))]
    public abstract class Tank : DamagingObject, IMovable
    {
        [SerializeField] Image weaponBar;

		[Header("Movement")]
		[SerializeField] float movementSpeed = 1;
		[SerializeField] float inverseMoveMult = .5f;
		[SerializeField] float rotationSpeed = 90;
		Rigidbody2D body;

        [Header("Tower")]
        [SerializeField] Transform tower;
        [SerializeField] bool canRotateTower = true;
        [SerializeField] float towerRotationSpeed = 150;

        [Header("Weapon")]
		[SerializeField] Transform[] weaponBases;
        [SerializeField] Weapon[] availableWeapons;
		int currentWeaponId = 0;
		Weapon[] installedWeapons;

		protected override void Awake()
		{
            base.Awake();
            installedWeapons = new Weapon[weaponBases.Length];
            body = GetComponent<Rigidbody2D>();
		}

        void Update()
		{
			weaponBar.transform.parent.gameObject.SetActive(installedWeapons[0] != null && installedWeapons[0].ReloadingProgress < 1 && installedWeapons[0].ReloadingDuration >= 1);
			if (installedWeapons[0] != null)
			{
				weaponBar.fillAmount = installedWeapons[0].ReloadingProgress;
			}
		}

		public void Init()
		{
            currentWeaponId = 0;
			LoadWeapon();
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

        public void Move(float direction) => body.MovePosition(transform.position + (transform.up * direction).normalized * movementSpeed * math.abs(direction) * Time.deltaTime * (direction >= 0 ? 1 : inverseMoveMult));

        public void Rotate(float direction)
		{
			body.MoveRotation(transform.eulerAngles.z - rotationSpeed * direction * Time.deltaTime);
		}

        #endregion

        public void ProcessTowerRotation(Vector2 mousePosition)
        {
            if (!canRotateTower)
            {
                return;
            }
            var targetDirection = transform.InverseTransformPoint(mousePosition).normalized;
            var targetAngle = math.degrees(math.atan2(targetDirection.y, targetDirection.x)) - 90;
            tower.localEulerAngles = Vector3.forward * Mathf.MoveTowardsAngle(tower.localEulerAngles.z, targetAngle, towerRotationSpeed * Time.deltaTime);
        }
    }
}
