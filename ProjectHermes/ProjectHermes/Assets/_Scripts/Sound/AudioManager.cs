using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Volume volume;

	[PropertySpace]

	[Searchable] public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}

	}

	private void OnLevelWasLoaded(int level)
	{
		foreach (Sound s in sounds)
		{
			if (SceneManager.GetActiveScene().name == s.SceneQueue)
			{
				print(s.name);
				Play(s.name);
	
			}
			else if (SceneManager.GetActiveScene().name == s.EndSceneQueue)
			{
				if(s.SceneQueueTransitionTime == 0)
				{
					s.SceneQueueTransitionTime = s.source.clip.length - s.source.time;
				}
				StartCoroutine(StartFade(s.source, s.SceneQueueTransitionTime, 0));
			}
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		// Music 
		if (s.volumeType) s.source.volume = s.volume * volume.musicVolume * volume.masterVolume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));

		// Sound Effects 
		if (!s.volumeType) s.source.volume = s.volume * volume.soundEffectVolume * volume.masterVolume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));



		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
		//print(s.source);
		s.source.time = s.StartAt;

		if(s.hasStopTime)
		{
			StartCoroutine(WaitForStop(s, s.StopAt));
		}


		//  Transition
		//-------------
		if (s.HasTransition)
		{
			if (s.transitionAfter == 0) s.transitionAfter = s.source.clip.length;
			StartCoroutine(WaitForTransition(s, s.transitionAfter - s.StartAt, s.transitionTo));
		}
	}

	public void StopAllSounds()
	{
		foreach (Sound s in sounds)
		{
			s.source.Stop();
		}
	}

	IEnumerator WaitForTransition(Sound s, float soundLength, string transitionName)
	{
		yield return new WaitForSeconds(soundLength);

		
		StartCoroutine(StartFade(s.source, s.transitionLength, s.transitionTargetVolume));

		Play(transitionName);
	}

	IEnumerator WaitForStop(Sound s, float stopTime)
	{
		yield return new WaitForSeconds(stopTime);

		
		s.source.Stop();
	}

	IEnumerator QueueTrasition(Sound s, float TimeUltilSong)
	{
		yield return new WaitForSeconds(TimeUltilSong);


		s.source.Play();
	}

	private void Update()
	{
		volume.masterVolume = volume.MasterVolumeSlider.value;
		volume.soundEffectVolume = volume.SoundEffectVolumeSlider.value;
		volume.musicVolume = volume.MusicVolumeSlider.value;

		ChangeVolume();
	}
	public void ChangeVolume()
	{
		foreach (Sound s in sounds)
		{
			// Music 
			if (s.volumeType) s.source.volume = s.volume * volume.musicVolume * volume.masterVolume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));

			// Sound Effects 
			if (!s.volumeType) s.source.volume = s.volume * volume.soundEffectVolume * volume.masterVolume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));

		}
	}
	public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
	{
		float currentTime = 0;
		float start = audioSource.volume;
		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
			yield return null;
		}
		if(targetVolume == 0) audioSource.Stop();
		yield break;
	}
}

