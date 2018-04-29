using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game
{
	public class Firearm : Weapon
	{
		public override void Shoot()
		{
            if (lastReloadTime + reloadDuration > Time.time)
            {
                return;
            }
            base.Shoot();
			
			var hit = Physics2D.RaycastAll(startShootPoint.position, startShootPoint.up)
				.Where(h => h.collider.GetComponent<Tank>() == null)
				.OrderBy(h => h.distance)
				.FirstOrDefault();
			if (hit)
			{
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, hit.point, Quaternion.identity);
                }
			}
		}
	}
}
