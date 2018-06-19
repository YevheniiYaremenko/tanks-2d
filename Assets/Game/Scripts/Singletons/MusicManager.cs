using System.Collections.Generic;
using UnityEngine;

namespace Game.Sound
{
	public enum MusicType
	{
		Menu,
		Game,
		Lose,
		Win,
	}

	[RequireComponent(typeof(AudioSource))]
    public class MusicManager : Singleton<MusicManager>
    {
        [SerializeField] AudioClip menuMusic;
        [SerializeField] AudioClip gameMusic;
        [SerializeField] AudioClip winMusic;
        [SerializeField] AudioClip loseMusic;

		AudioSource source;
		Dictionary<MusicType, AudioClip> musicMap;

		void Awake()
		{
			source = GetComponent<AudioSource>();
			musicMap = new Dictionary<MusicType, AudioClip>()
			{
                { MusicType.Menu, menuMusic },
                { MusicType.Game, gameMusic },
                { MusicType.Lose, loseMusic },
                { MusicType.Win, winMusic },
			};
		}

		public static void Play(MusicType type)
		{
			Instance.PlayMusic(type);
		}

		void PlayMusic(MusicType type)
		{
			source.loop = type == MusicType.Game || type == MusicType.Menu;
			source.clip = musicMap[type];
			source.Play();
		}
    }
}
