﻿using Unity.Mathematics;
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
        SpriteRenderer spriteRenderer;

        [Header("UI")]
		[SerializeField] GameObject ui;
        [SerializeField] Image healthBar;

		protected virtual void Awake()
		{
            Health = maxHealth;
            spriteRenderer = GetComponent<SpriteRenderer>();
			if (healthBar != null && this is AI.Enemy)
			{
				healthBar.transform.parent.localScale = new Vector3( math.sqrt(Health / 100f), 1, 1);
			}
		}

        #region IDamaging

        public float Health { get; private set; }
        public event System.Action<bool> onDeath;

        public void DealDamage(float damage)
        {
            Health = math.max(Health - damage * (1 - defence), 0);
            if (Health == 0)
            {
                Death();
            }
            else if (sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[math.min(sprites.Length - 1, (int)((maxHealth - Health) / maxHealth * sprites.Length))];
            }

            if (healthBar != null)
            {
				healthBar.fillAmount = Health / maxHealth;
            }
        }

        public virtual void Death()
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            if (deathSprite != null)
            {
                spriteRenderer.sprite = deathSprite;
            }
            spriteRenderer.sortingOrder = 0;

			if (ui != null)
			{
                ui.SetActive(false);
			}
            GetComponent<Collider2D>().enabled = false;
            Destroy(this);

            if (onDeath != null)
            {
                onDeath.Invoke(Health == 0);
            }
        }

        #endregion

    }
}
