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

            var hits = Physics2D.RaycastAll(startShootPoint.position, startShootPoint.up)
                .Where(h => h.collider.GetComponent<Tank>() == null)
                .OrderBy(h => h.distance)
                .ToArray();
            foreach(var hit in hits)
            {
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, hit.point, Quaternion.identity);
                }

                var damageHandler = hit.collider.GetComponent<IDamaging>();
                if (damageHandler != null)
                {
                    damageHandler.DealDamage(damage);
                }
            }
		}
	}
}
