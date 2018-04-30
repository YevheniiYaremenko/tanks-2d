using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DamagingObject : MonoBehaviour, IDamaging
    {
		[Header("Damaging")]
        [SerializeField] protected float maxHealth = 100;
        [Range(0f, 1f)] [SerializeField] protected float defence = 0.5f;

		[Header("View")]
		[SerializeField] Sprite[] sprites;
        [SerializeField] GameObject explosionEffect;
        [SerializeField] Sprite deathSprite;
        SpriteRenderer renderer;

        [Header("UI")]
        [SerializeField] Image healthBar;
        [SerializeField] Image weaponBar;

		void Awake()
		{
            Health = maxHealth;
            renderer = GetComponent<SpriteRenderer>();
		}

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

            if (healthBar != null)
            {

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
