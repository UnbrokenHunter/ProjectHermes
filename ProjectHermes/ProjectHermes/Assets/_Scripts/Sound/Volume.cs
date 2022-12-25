using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public class Volume
{
	// -----------------
	//      Master
	// -----------------

	// Master Volume
	[HorizontalGroup("Master Settings", 1f)]
	[BoxGroup("Master Settings/Master Volume")]
	[LabelWidth(160)]
	[Range(0f, 3f)]
	public float masterVolume = 1;
	
	// Master Volume Slider
	[HorizontalGroup("Master Settings", 1f)]
	[BoxGroup("Master Settings/Master Volume")]
	[LabelWidth(180)]
	public Slider MasterVolumeSlider;

	// -----------------
	//   Sound Effects
	// -----------------

	// Sound Effect Volume
	[HorizontalGroup("Volume Settings", 0.5f)]
	[BoxGroup("Volume Settings/Sound Effects")]
	[LabelWidth(160)]
	[Range(0f, 3f)]
	public float soundEffectVolume = 1;

	// Sound Effect Slider
	[HorizontalGroup("Volume Settings", 0.5f)]
	[BoxGroup("Volume Settings/Sound Effects")]
	[LabelWidth(180)]
	public Slider SoundEffectVolumeSlider;

	// -----------------
	//      Music
	// -----------------

	// Music Volume 
	[BoxGroup("Volume Settings/Music")]
	[LabelWidth(160)]
	[Range(0f, 3f)]
	public float musicVolume = 1;

	// Music Volume Slider
	[BoxGroup("Volume Settings/Music")]
	[LabelWidth(180)]
	public Slider MusicVolumeSlider;

}


