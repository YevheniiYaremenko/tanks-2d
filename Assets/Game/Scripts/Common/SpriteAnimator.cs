using Unity.Mathematics;
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
		[SerializeField] float startDelay = 0;
		[SerializeField] bool playOnAwake = true;
		[SerializeField] bool loop;
		[SerializeField] bool autoDestroy;
		
		bool isPlaying = false;
		int currentFrame = 0;
		float lastFrameTime = 0;
		SpriteRenderer spriteRenderer;
		AnimatedSprite currentAnimation;

		void Awake()
		{
            spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void Play()
		{
            isPlaying = true;
            lastFrameTime = Time.time;
			spriteRenderer.enabled = true;
            currentAnimation = animations.Random();
			currentFrame = 0;
            spriteRenderer.sprite = currentAnimation.frames[currentFrame];
		}

		void Update()
		{
            if (playOnAwake && startDelay > 0)
            {
                startDelay -= Time.deltaTime;
            }
			else if (playOnAwake && !isPlaying)
			{
                Play();
			}

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
					spriteRenderer.enabled = false;
				}

				return;
			}

			currentFrame = (currentFrame + (int)(math.max(1, Time.deltaTime * fps))) % currentAnimation.frames.Length;
			lastFrameTime = Time.time;

			spriteRenderer.sprite = currentAnimation.frames[currentFrame];
		}
    }
}
