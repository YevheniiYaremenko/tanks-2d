using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
	[RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
		[SerializeField] Sprite[] frames;
		[SerializeField] float fps = 10;
		[SerializeField] bool playOnAwake = true;
		[SerializeField] bool loop;
		[SerializeField] bool autoDestroy;
		
		bool isPlaying = false;
		int currentFrame = 0;
		float lastFrameTime = 0;
		SpriteRenderer renderer;

		void Awake()
		{
            renderer = GetComponent<SpriteRenderer>();
			if (playOnAwake)
			{
				Play();
			}
		}

		public void Play()
		{
            isPlaying = true;
            lastFrameTime = Time.time;
			renderer.enabled = true;
		}

		void Update()
		{
			if (!isPlaying || Time.time < lastFrameTime + 1 / fps)
			{
				return;
			}

			if (currentFrame == frames.Length - 1 && !loop)
			{
                isPlaying = false;

				if (autoDestroy)
				{
					Destroy(gameObject);
				}
				else
				{
					renderer.enabled = false;
				}

				return;
			}

			currentFrame = (currentFrame + (int)(Mathf.Max(1, Time.deltaTime * fps))) % frames.Length;
			lastFrameTime = Time.time;

			renderer.sprite = frames[currentFrame];
		}
    }
}
