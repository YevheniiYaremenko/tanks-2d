using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game
{
	public class Laser : Weapon
	{
		public override void Shoot()
		{
            if (lastReloadTime + reloadDuration > Time.time)
            {
                return;
            }
            base.Shoot();

            int layerMask = 1 << 8 | 1 << 11; //cast only obstacle and enemy layers
            var hits = Physics2D.RaycastAll(startShootPoint.position, startShootPoint.up, Mathf.Infinity, layerMask)
                .OrderBy(h => h.distance)
                .ToArray();

            var damagingTargets = new List<RaycastHit2D>();
            foreach(var hit in hits)
            {
                var damaging = hit.collider.GetComponent<IDamaging>();
                if (damaging == null)
                {
                    break;
                }
                damagingTargets.Add(hit);
            }
            foreach(var target in damagingTargets)
            {
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, target.point, Quaternion.identity);
                }
                target.collider.GetComponent<IDamaging>().DealDamage(damage);
            }
		}
	}
}
