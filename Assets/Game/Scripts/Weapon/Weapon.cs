using System.Collections;
using System.Collections.Generic;
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

      protected float lastReloadTime = 0;

      public float ReloadingProgress { get { return Mathf.Clamp01((Time.time - lastReloadTime) / reloadDuration);} }

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
      }
    }
}
