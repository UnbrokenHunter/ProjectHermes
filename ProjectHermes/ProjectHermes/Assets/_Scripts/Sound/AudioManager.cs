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

	private void Awake()
	{
		// Singleton Workflow
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		// Instantiate all of the AudioSources
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.volumeBackup = s.volume;
			s.source.playOnAwake = false;
			s.source.outputAudioMixerGroup = mixerGroup;
		}

	}

	private void Start()
	{
		CheckScene();
	}

	/// <summary>
	/// 
	///  Explanation:
	///  When entering a new scene, check to see if there are any sounds that are supposted to play when doing so.
	///  If there are, make sure that they are not already playing, then play them. 
	///  Also stop playing any sounds that are not supposed to play when entering the scene.
	///  
	/// </summary>
	/// <param name="level"></param>
	private void OnLevelWasLoaded(int level)
	{
		CheckScene();
	}

	private void CheckScene()
	{
		foreach (Sound s in sounds)
		{
			if (s.isEffectedBySceneTransition)
			{
				bool fadeOut = true;

				foreach (string scene in s.ScenesToPlay)
				{

					if (SceneManager.GetActiveScene().name == scene && fadeOut)
					{
						//print(s.name + "---------------------------------------" + s.source.isPlaying);
						fadeOut = false;

						if (!s.source.isPlaying)
						{
							if(s.fadeIn)
								FadeInVolume(s.name, s.SceneToPlayTransitionTime);
							else
								Play(s.name);
						}

					}

				}
				if (fadeOut && s.source.isPlaying)
				{

					if (s.SceneToPlayTransitionTime == 0)
						s.SceneToPlayTransitionTime = s.source.clip.length - s.source.time;

					FadeOutVolume(s.name, s.SceneToPlayTransitionTime);

				}
			}
		}
	}

	public static Sound FindSound(string name) 
	{
		Sound s = Array.Find(AudioManager.instance.sounds, item => item.name == name);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return null;
		}

		return s;
	}

	#region Playing Clip

	public void Play(string sound)
	{
		Sound s = FindSound(sound);
		if (s == null) return;

		if (!s.source.isPlaying || s.canOverplay) {
			
			// Pitch 
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

			s.source.Play();

			//print("The audio track: " + s.name + " is now playing.");

			s.source.time = s.AudioStartAt;

			if (s.loop)
			{
				s.isLooping = true;
				StartCoroutine(LoopAudio(s));
			}

			if (s.hasStopTime)
				StartCoroutine(HasStopTime(s, s.StopAt));
		}

		else 
			print(s.name + " was not played because it is already playing");

	}

	public void Stop(string sound)
	{
		Sound s = FindSound(sound);
		if (s == null) return;

		s.source.Stop();
		s.isLooping = false;
	}

	public void Pause(string sound)
	{
		Sound s = FindSound(sound);
		if (s == null) return;

		s.source.Pause();
	}
	
	public void Unpause(string sound)
	{
		Sound s = FindSound(sound);
		if (s == null) return;

		s.source.UnPause();
	}

	public void Restart(string sound)
	{
		Sound s = FindSound(sound);
		if (s == null) return;

		print(s.name + " was restarted");

		s.source.Stop();
		s.source.Play();
	}



	#endregion

	#region Play Effectors

	/// <summary>
	/// 
	/// A custom audio looper, to allow for custom loop start and stops
	/// 
	/// </summary>
	/// <returns></returns>
	private IEnumerator LoopAudio(Sound s)
	{
		if (!s.source.isPlaying) yield return null;

		// If loop StopAt is not 0, use it, otherwise use the clip length
		yield return new WaitForSeconds(s.loopStopAt != 0 ? s.loopStopAt - s.LoopStartAt : s.clip.length - s.LoopStartAt);

		print("Loop Started");

		Restart(s.name);			
		s.source.time = s.LoopStartAt;

		StartCoroutine(LoopAudio(s));

	}
	
	private IEnumerator HasStopTime(Sound s, float stopTime)
	{
		yield return new WaitForSeconds(stopTime);

		Stop(s.name);
	}

	#endregion

	public void StopAllSounds()
	{
		foreach (Sound s in sounds)
		{
			Stop(s.name);
		}
	}

	/// <summary>
	/// 
	/// Sound that is currently playing
	/// Sound to be played
	/// 
	/// </summary>
	private void TransitionTwoAudio(string currentSound, string nextSound, float transitionLength)
	{
		FadeOutVolume(currentSound, transitionLength);
		FadeInVolume(nextSound, transitionLength);
	}

	#region Volume
	
	/// <summary>
	/// 
	/// Explanation: 
	/// Volume Type is a boolean. 
	/// True represents Music, false represents Sound Effect
	/// 
	/// </summary>
	private void ChangeVolume()
	{
		foreach (Sound s in sounds)
		{		
			// Music 
			if (s.volumeType) s.source.volume = s.volume * volume.musicVolume * volume.masterVolume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));

			// Sound Effects 
			if (!s.volumeType) s.source.volume = s.volume * volume.soundEffectVolume * volume.masterVolume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		}
	}
	
	private void Update()
	{
		volume.masterVolume = volume.MasterVolumeSlider.value;
		volume.musicVolume = volume.MusicVolumeSlider.value;
		volume.soundEffectVolume = volume.SoundEffectVolumeSlider.value;

		ChangeVolume();
	}

	#endregion

	#region Volume Fading

	public void FadeInVolume(string audioName, float duration)
	{
		StartCoroutine(FadeIn(audioName, duration));
	}

	public void FadeOutVolume(string audioName, float duration)
	{
		StartCoroutine(FadeOut(audioName, duration));
	}

	/// <summary>
	/// 
	/// Fades In a sound
	/// 
	/// </summary>
	/// <param name="source"></param> The sound to be faded in
	/// <param name="fadeDuration"></param> the time it takes to fade
	/// <param name="targetVolume"></param> the desired volume after the fade
	/// <returns></returns>
	public static IEnumerator FadeIn(string audioName, float duration)
	{
		//print(audioName + " is Fading In");

		// Find sound
		Sound s = FindSound(audioName);
		if (s == null) yield break;

		if(s.source.isPlaying) yield break;

		float currentTime = 0;
		float start = s.volumeBackup;

		// Play Sound
		instance.Play(audioName);

		s.volume = 0;
		if (s.FadeInIgnoresStartTime) s.source.time = 0;


		while (currentTime < duration)
		{
			print("Vol: " + s.volume + " Start: " + start + " Math: " + (currentTime / duration) + " Cur: " + currentTime + " Dur: " + duration);
			currentTime += Time.deltaTime;
			s.volume = Mathf.Lerp(0, start, currentTime / duration);
			yield return null;
		}

	}


	/// <summary>
	/// 
	/// Fades out a sound
	/// 
	/// </summary>
	/// <param name="source"></param> The sound to be faded out
	/// <param name="fadeDuration"></param> the time it takes to fade
	/// <param name="targetVolume"></param> the desired volume after the fade
	/// <returns></returns>
	private static IEnumerator FadeOut(string audioName, float duration)
	{
		print(audioName + " is Fading Out");

		// Find Sound
		Sound s = FindSound(audioName);
		if (s == null) yield break;

		float currentTime = 0;
		float start = s.volume;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			s.volume = Mathf.Lerp(start, 0, currentTime / duration);
			yield return null;
		}

		// Stop Audio
		if (s.volume <= 0)
		{
			AudioManager.instance.Stop(audioName);
			s.volume = start;
		}

		yield break;
	}

	#endregion

}

