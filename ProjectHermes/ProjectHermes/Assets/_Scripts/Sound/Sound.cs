using UnityEngine.Audio;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

[System.Serializable] 
public class Sound {
	
	[FoldoutGroup("$name")]

	// Name

	[Title("$name", "", TitleAlignments.Left, true, true)]
	[LabelWidth(130)]
	public string name;


	// -------------
	//    Clip Settings
	// -------------


	// Clip
	[HorizontalGroup("$name/Split")]
	[VerticalGroup("$name/Split/Left")]
	[BoxGroup("$name/Split/Left/Audio")]
	[LabelWidth(130)]
	[LabelText("Audio Clip")]
	public AudioClip clip;

	// Mixer Group
	[BoxGroup("$name/Split/Left/Audio")]
	[LabelWidth(130)]
	public AudioMixerGroup mixerGroup;


	// -------------
	//    Effectors
	// -------------


	// Volume Type
	[BoxGroup("$name/Split/Left/Effectors")]
	[ValueDropdown("_volumeType")]
	[LabelWidth(170)]
	public bool volumeType;

	// -------------
	//   Start And Stop Times
	// -------------

	// Fade In
	[BoxGroup("$name/Split/Left/Effectors")]
	[LabelWidth(170)]
	[Tooltip("Whether or not fading in should ignore the audio start time")]
	[LabelText("Fade in Ignores Start Time?")]
	public bool FadeInIgnoresStartTime = false;

	// Start At
	[BoxGroup("$name/Split/Left/Effectors")]
	[LabelWidth(170)]
	[Tooltip("The timestamp of when the Audio should start at")]
	[LabelText("Start Audio At: ")]
	public float AudioStartAt = 0;

	// Has Stop Time
	[BoxGroup("$name/Split/Left/Effectors")]
	[LabelWidth(170)]
	public bool hasStopTime = false;

	// Stop At
	[BoxGroup("$name/Split/Left/Effectors")]
	[ShowIf("hasStopTime")]
	[LabelWidth(170)]
	public float StopAt = 0;

	// -------------
	//    Loop
	// -------------

	[BoxGroup("$name/Split/Left/Loop")]
	[LabelWidth(130)]
	[Tooltip("Whether or not this audio track should loop")]
	[LabelText("Loop Audio?")]
	public bool loop = false;

	[HideInInspector]
	public bool isLooping = false;

	[ShowIfGroup("$name/Split/Left/Loop/loop")]
	[ShowIf("loop")]
	[LabelWidth(130)]
	[LabelText("Start Loop At: ")]
	[Tooltip("The timestamp of when the Loop should start at")]
	public float LoopStartAt = 0;

	// Stop At
	[ShowIfGroup("$name/Split/Left/Loop/loop")]
	[LabelWidth(130)]
	public float loopStopAt = 0;


	// -------------
	//	  Pitch
	// -------------

	[BoxGroup("$name/Split/Right/Pitch")]

	// Pitch
	[BoxGroup("$name/Split/Right/Pitch")]
	[LabelWidth(130)]
	[Range(0f, 2f)]
	public float pitch = 1f;


	// Pitch Variance
	[BoxGroup("$name/Split/Right/Pitch")]
	[LabelWidth(130)]
	[Range(0f, 1f)]
	public float pitchVariance = .1f;




	// -------------
	//    Volume
	// -------------

	[VerticalGroup("$name/Split/Right")]
	[BoxGroup("$name/Split/Right/Volume")]
	// Volume
	[VerticalGroup("$name/Split/Right")]
	[BoxGroup("$name/Split/Right/Volume")]
	[LabelWidth(130)]
	[Range(0f, 3f)]
	public float volume = .75f;

	[HideInInspector]
	public float volumeBackup;

	// Volume Variance
	[VerticalGroup("$name/Split/Right")]
	[BoxGroup("$name/Split/Right/Volume")]
	[LabelWidth(130)]
	[Range(0f, 1f)]
	public float volumeVariance = .1f;


	// -------------
	//  Transitions
	// -------------

	[PropertyTooltip("For Transitions between specific scenes")]
	[BoxGroup("$name/Split/Right/Transitions")]

	// Scene Queues
	[BoxGroup("$name/Split/Right/Transitions")]
	[LabelWidth(190)]
	[Tooltip("This audio will play when on this scene")]
	public string[] ScenesToPlay = { };

	// Scene Queues Transition Time
	[BoxGroup("$name/Split/Right/Transitions")]
	[LabelWidth(190)]
	[LabelText("Transition Time")]
	[Tooltip("The transition time between this scene and the previous one")]
	public float SceneToPlayTransitionTime = 1;

	[BoxGroup("$name/Split/Right/Transitions")]
	[Tooltip("If given the option, should the audio effect fade in, or start instantly?")]
	[LabelWidth(190)]
	[LabelText("Fade In?")]
	public bool fadeIn = true;

	[BoxGroup("$name/Split/Right/Transitions")]
	[LabelWidth(190)]
	[Tooltip("Can a sound be played multiple times, or should it not play again while it is already playing")]
	[LabelText("Can Overplay?")]
	public bool canOverplay = true;

	[BoxGroup("$name/Split/Right/Transitions")]
	[LabelWidth(190)]
	[LabelText("Is Effected by Transitions?")]
	[Tooltip("Whether or not this audio should be played/stopped when entering a new scenes")]
	public bool isEffectedBySceneTransition = false;

	// -----------------
	//    Transitions
	// -----------------

	#region Transitions

	[FoldoutGroup("$name")]
	[Title("Transition Settings", "", TitleAlignments.Left, true, true)]
	[HorizontalGroup("$name/Transition")]
	[LabelWidth(130)]
	public bool HasTransition;


	[PropertySpace]


	// -------------
	//  Transition Clip
	// -------------

	[ShowIf("HasTransition")]
	[HorizontalGroup("$name/Split2")]
	[VerticalGroup("$name/Split2/Left2")]
	[BoxGroup("$name/Split2/Left2/Transition Clip")]

	// Transition Clip
	[ShowIf("HasTransition")]
	[HorizontalGroup("$name/Split2")]
	[VerticalGroup("$name/Split2/Left2")]
	[BoxGroup("$name/Split2/Left2/Transition Clip")]
	[LabelWidth(160)]
	public string transitionTo = "Clip";

	// Transition Length
	[ShowIf("HasTransition")]
	[HorizontalGroup("$name/Split2")]
	[VerticalGroup("$name/Split2/Left2")]
	[BoxGroup("$name/Split2/Left2/Transition Clip")]
	[LabelWidth(160)]
	public float transitionLength;


	// -------------
	//    
	// -------------

	[VerticalGroup("$name/Split2/Right2")]
	[BoxGroup("$name/Split2/Right2/Effectors")]

	// Volume Type
	[VerticalGroup("$name/Split2/Right2")]
	[ShowIf("HasTransition")]
	[BoxGroup("$name/Split2/Right2/Effectors")]
	[LabelWidth(130)]
	public float Aa;


	// -------------
	//	Transition After
	// -------------

	[BoxGroup("$name/Split2/Right2/Transition After")]

	// Transition After
	[ShowIf("HasTransition")]
	[BoxGroup("$name/Split2/Right2/Transition After")]
	[LabelWidth(130)]
	[Tooltip("Transition to next sound after ___ seconds. If left at 0, time will be set the the length of the clip")]
	public float transitionAfter = 0;

	#endregion

	// -------------
	//    Hidden
	// -------------

	[HideInInspector]
	public AudioSource source;

	// -------------
	//    Other
	// -------------

	private static IEnumerable _volumeType = new ValueDropdownList<bool>()
	{
	{ "Music", true },
	{ "Sound Effect", false },
	};

}







