using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public abstract class Enemy : MonoBehaviour, IDamaging, IMovable
    {
        [SerializeField] float maxHealth = 100;
        [Range(0f, 1f)] [SerializeField] float defence = 0.5f;
        [SerializeField] float movementSpeed = 1;
        [SerializeField] float rotationSpeed = 90;
		[SerializeField] float damage = 100;

		[SerializeField] Sprite[] sprites;

		[SerializeField] GameObject explosionEffect;
		[SerializeField] Sprite deathSprite;

        SpriteRenderer renderer;
		Transform target;

        void Awake()
        {
            Health = maxHealth;
			renderer = GetComponent<SpriteRenderer>();
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

        #region IDamaging

        public float Health { get; private set; }
        public event System.Action onDeath;

        public void DealDamage(float damage)
        {
            Health = Mathf.Max(Health - damage * (1 - defence), 0);
            if (Health == 0)
            {
                Death();
            }
			else
			{
                renderer.sprite = sprites[Mathf.Min(sprites.Length - 1, (int)((maxHealth - Health) / maxHealth * sprites.Length))];
			}
        }

        public void Death()
        {
            if (onDeath != null)
            {
                onDeath.Invoke();
            }

			if (explosionEffect != null)
			{
				Instantiate(explosionEffect, transform.position, Quaternion.identity);
			}

			if (deathSprite != null)
			{
				renderer.sprite = deathSprite;
			}

			GetComponent<Collider2D>().enabled = false;
			Destroy(this);
        }

        #endregion
    }
}
