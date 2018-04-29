using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Weapon : MonoBehaviour
    {
      [SerializeField] protected Transform startShootPoint;
      [SerializeField] protected float damage;
      [SerializeField] protected float reloadDuration;

      [SerializeField] protected GameObject shootEffect;
      [SerializeField] protected GameObject hitEffect;

      protected float lastReloadTime = 0;

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
