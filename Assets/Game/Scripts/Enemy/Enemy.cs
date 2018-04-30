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
		Transform target;

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

		void OnTriggerEnter2D(Collider2D col)
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
            transform.position += (transform.up * direction).normalized * movementSpeed * Mathf.Abs(direction) * Time.deltaTime;
        }

        public void Rotate(float direction)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * direction * Time.deltaTime);
        }

        #endregion
    }
}
