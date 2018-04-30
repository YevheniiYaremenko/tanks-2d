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
		[SerializeField] GameObject ui;
        [SerializeField] Image healthBar;

		protected virtual void Awake()
		{
            Health = maxHealth;
            renderer = GetComponent<SpriteRenderer>();
			if (healthBar != null && this is AI.Enemy)
			{
				healthBar.transform.parent.localScale = new Vector3( Mathf.Sqrt(Health / 100f), 1, 1);
			}
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
            else if (sprites.Length > 0)
            {
                renderer.sprite = sprites[Mathf.Min(sprites.Length - 1, (int)((maxHealth - Health) / maxHealth * sprites.Length))];
            }

            if (healthBar != null)
            {
				healthBar.fillAmount = Health / maxHealth;
            }
        }

        public void Death()
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            if (deathSprite != null)
            {
                renderer.sprite = deathSprite;
            }

			if (ui != null)
			{
                ui.SetActive(false);
			}
            GetComponent<Collider2D>().enabled = false;
            Destroy(this);

            if (onDeath != null)
            {
                onDeath.Invoke();
            }
        }

        #endregion

    }
}
