using UnityEngine;

namespace Game
{
    public abstract class Weapon : MonoBehaviour, IShootable
    {
      [SerializeField] protected Transform startShootPoint;
      [SerializeField] protected float damage;
      [SerializeField] protected float reloadDuration;

      [SerializeField] protected GameObject shootEffect;
      [SerializeField] protected GameObject hitEffect;
	  [SerializeField] AudioClip shotSound;

      protected float lastReloadTime = 0;
	  AudioSource shootinsSoundSource;

      public float ReloadingProgress { get { return Mathf.Clamp01((Time.time - lastReloadTime) / reloadDuration); } }
      public float ReloadingDuration { get { return reloadDuration; } }
      public virtual bool Automatic { get { return false; } }

        void Awake()
        {
            lastReloadTime = Time.time;
            shootinsSoundSource = GetComponent<AudioSource>();
        }

        public virtual void Shoot()
        {
            if (lastReloadTime + reloadDuration > Time.time)
            {
                return;
            }
            lastReloadTime = Time.time;
            if (shootEffect != null)
            {
                Instantiate(shootEffect, startShootPoint.position, startShootPoint.rotation, startShootPoint);
            }
            shootinsSoundSource.PlayOneShot(shotSound);
        }
    }
}
