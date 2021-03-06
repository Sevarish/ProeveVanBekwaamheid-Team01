using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
	// Singleton instance.
	public static SoundManager Instance = null;

	// Audio players components.
	public AudioSource EffectsSource;
	public AudioSource MusicSource;
	private Dictionary<AudioClip, AudioSource> audioSources = new Dictionary<AudioClip, AudioSource>();

	// Music tracks
	public AudioClip[] musicTracks;

	[Range(0, 1)]
	public float effectsVolume, musicVolume;

	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		gameObject.transform.position = Camera.main.transform.position;

		if (SceneManager.GetActiveScene().name != "Game")
        {
			// Fade out music
			MusicSource.volume -= 0.167f;
			if (MusicSource.volume <= 0)
			{
				MusicSource.Stop();
			}
		}
		else
        {
			if (!MusicSource.isPlaying && musicTracks != null)
			{
				RandomMusic(musicTracks);
			}
		}
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip)
	{
		bool hasClip = audioSources.ContainsKey(clip);
        if (hasClip)
        {
			audioSources[clip].Play();
        }
        else
        {
			AudioSource newSrc = gameObject.AddComponent<AudioSource>();
			newSrc.volume = effectsVolume;
			newSrc.clip = clip;
			newSrc.playOnAwake = false;
			newSrc.loop = false;
			audioSources.Add(clip, newSrc);
			audioSources[clip].Play();
        }
	}
	public void Play(AudioClip clip, float volume)
	{
		bool hasClip = audioSources.ContainsKey(clip);
		if (hasClip)
		{
			audioSources[clip].Play();
		}
		else
		{
			AudioSource newSrc = gameObject.AddComponent<AudioSource>();
			newSrc.volume = volume;
			newSrc.clip = clip;
			newSrc.playOnAwake = false;
			newSrc.loop = false;
			audioSources.Add(clip, newSrc);
			audioSources[clip].Play();
		}
	}

	public void StopSoundEffect(AudioClip clip)
    {
		audioSources[clip].Stop();
    }

	public void StopMusic()
    {
		MusicSource.Stop();
    }

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip)
	{
		MusicSource.volume = musicVolume;
		MusicSource.clip = clip;
		MusicSource.Play();
	}

	// Play a random clip from an array, and randomize the pitch slightly.
	public void RandomSoundEffect(AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);

		Play(clips[randomIndex]);
	}

	public void RandomMusic(AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);

		MusicSource.volume = musicVolume;
		MusicSource.clip = clips[randomIndex];
		MusicSource.Play();
	}

}