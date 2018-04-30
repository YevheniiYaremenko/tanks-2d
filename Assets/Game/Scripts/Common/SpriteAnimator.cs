﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
	[System.Serializable]
	public class AnimatedSprite
	{
		public Sprite[] frames;
	}

	[RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
		[SerializeField] AnimatedSprite[] animations;
		[SerializeField] float fps = 10;
		[SerializeField] bool playOnAwake = true;
		[SerializeField] bool loop;
		[SerializeField] bool autoDestroy;
		
		bool isPlaying = false;
		int currentFrame = 0;
		float lastFrameTime = 0;
		SpriteRenderer renderer;
		AnimatedSprite currentAnimation;

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
            currentAnimation = animations[Random.Range(0, animations.Length)];
			currentFrame = 0;
            renderer.sprite = currentAnimation.frames[currentFrame];
		}

		void Update()
		{
			if (!isPlaying || Time.time < lastFrameTime + 1 / fps)
			{
				return;
			}

			if (currentFrame == currentAnimation.frames.Length - 1 && !loop)
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

			currentFrame = (currentFrame + (int)(Mathf.Max(1, Time.deltaTime * fps))) % currentAnimation.frames.Length;
			lastFrameTime = Time.time;

			renderer.sprite = currentAnimation.frames[currentFrame];
		}
    }
}