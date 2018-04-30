using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
	[RequireComponent(typeof(Camera))]
    public class CameraFollower : MonoBehaviour
    {
		[SerializeField] Vector4 followArea = new Vector4(0.3f, 0.3f, 0.7f, 0.7f);
		
		Transform target;

		public void SetTarget(Transform target)
		{
			this.target = target;
		}

		void Update()
		{
			if (target == null)
			{
				return;
			}

			var targetViewportPosition = Camera.main.WorldToViewportPoint(target.position);
			if (
				Mathf.Clamp(targetViewportPosition.x, followArea.x, followArea.z) == targetViewportPosition.x
				&& Mathf.Clamp(targetViewportPosition.y, followArea.y, followArea.w) == targetViewportPosition.y)
			{
				return;
			}
			var cameraViewportPosition = new Vector3(
                0.5f - Mathf.Clamp(targetViewportPosition.x, followArea.x, followArea.z) + targetViewportPosition.x,
                0.5f - Mathf.Clamp(targetViewportPosition.y, followArea.y, followArea.w) + targetViewportPosition.y
			);
			var cameraWorldPosition = Camera.main.ViewportToWorldPoint(cameraViewportPosition);
			cameraWorldPosition.z = -1;
            transform.position = cameraWorldPosition;
		}
    }
}
