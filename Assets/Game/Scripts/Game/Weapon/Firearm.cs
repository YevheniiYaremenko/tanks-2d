using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game
{
	public class Firearm : Weapon
	{
		[SerializeField] bool automatic = false;
        public override bool Automatic { get { return automatic; } }

		public override void Shoot()
		{
            if (lastReloadTime + reloadDuration > Time.time)
            {
                return;
            }
            base.Shoot();
			
			int layerMask = 1 << 8 | 1 << 11; //cast only obstacle and enemy layers
			var hit = Physics2D.RaycastAll(startShootPoint.position, startShootPoint.up, 1000, layerMask)
				.OrderBy(h => h.distance)
				.FirstOrDefault();
			if (hit)
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
