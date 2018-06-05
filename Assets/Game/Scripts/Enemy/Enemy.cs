using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public abstract class Enemy : DamagingObject, IDamaging, IMovable
    {
        [Header("Movement")]
        [SerializeField] float movementSpeed = 1;
        [SerializeField] float rotationSpeed = 90;

        [Header("Enemy")]
		[SerializeField] float damage = 100;
		[SerializeField] int killBonus = 10;
		Transform target;
        Rigidbody2D body;

		public int KillBonus { get { return killBonus; } }

        protected override void Awake()
        {
            base.Awake();
            body = GetComponent<Rigidbody2D>();
        }

        public void SetData(Transform target)
        {
            this.target = target;
        }

        void Update()
		{
			if (target == null)
			{
				return;
			}

			var directionAngle = Vector2.SignedAngle(transform.up, ((Vector2)target.position - (Vector2)transform.position).normalized);
			Move(Mathf.Max(Mathf.InverseLerp(90, 0, Mathf.Abs(directionAngle)), .1f));
			Rotate(Mathf.Sign(directionAngle) * Mathf.InverseLerp(0, 180, Mathf.Abs(directionAngle)));
		}

		void OnCollisionEnter2D(Collision2D col)
		{
            if (col.transform == target)
            {
                var damageHandler = col.transform.GetComponent<IDamaging>();
                if (damageHandler != null)
                {
                    damageHandler.DealDamage(damage);
                }

                Death();
            }
		}

        #region IMovable

        public void Move(float direction)
        {
            body.MovePosition(transform.position + (transform.up * direction).normalized * movementSpeed * Mathf.Abs(direction) * Time.deltaTime);
        }

        public void Rotate(float direction)
        {
            body.MoveRotation(transform.eulerAngles.z + rotationSpeed * direction * Time.deltaTime);
        }

        #endregion
    }
}
