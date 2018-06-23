using Unity.Mathematics;
using UnityEngine;

namespace Game.Utils
{
	[RequireComponent(typeof(Camera))]
    public class CameraFollower : Singleton<CameraFollower>
    {
		[SerializeField] float4 followArea = new float4(0.3f, 0.3f, 0.7f, 0.7f);
		
		Transform target;
		Camera cam;

        ///<summary>
        /// Set the following Transform
		///<param name="target">Transform of the following object</param>
        ///</summary>
        public void SetTarget(Transform target)
		{
			this.target = target;
		}

        ///<summary>
        /// Center camera to following Transform
        ///</summary>
        public void Center()
        {
			if (target != null)
			{
                var cameraPosition = target.position;
                cameraPosition.z = -1;
                transform.position = cameraPosition;
			}
        }

		void Awake()
		{
			cam = GetComponent<Camera>();
		}

		void Update()
		{
			if (target == null)
			{
				return;
			}

			var targetViewportPosition = cam.WorldToViewportPoint(target.position);
			if (math.clamp(targetViewportPosition.x, followArea.x, followArea.z) == targetViewportPosition.x
				&& math.clamp(targetViewportPosition.y, followArea.y, followArea.w) == targetViewportPosition.y)
			{
				return;
			}
			var cameraViewportPosition = new Vector3(
                0.5f - math.clamp(targetViewportPosition.x, followArea.x, followArea.z) + targetViewportPosition.x,
                0.5f - math.clamp(targetViewportPosition.y, followArea.y, followArea.w) + targetViewportPosition.y
			);
			var cameraWorldPosition = cam.ViewportToWorldPoint(cameraViewportPosition);
			cameraWorldPosition.z = -1;
            transform.position = cameraWorldPosition;
		}
    }
}
