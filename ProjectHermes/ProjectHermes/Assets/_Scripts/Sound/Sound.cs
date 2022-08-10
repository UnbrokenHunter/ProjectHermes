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

	[HorizontalGroup("$name/Split")]
	[VerticalGroup("$name/Split/Left")]
	[BoxGroup("$name/Split/Left/Clip")]

	// Clip
	[HorizontalGroup("$name/Split")]
	[VerticalGroup("$name/Split/Left")]
	[BoxGroup("$name/Split/Left/Clip")]
	[LabelWidth(130)]
	public AudioClip clip;

	// Mixer Group
	[HorizontalGroup("$name/Split")]
	[VerticalGroup("$name/Split/Left")]
	[BoxGroup("$name/Split/Left/Clip")]
	[LabelWidth(130)]
	public AudioMixerGroup mixerGroup;


	// -------------
	//    Effectors
	// -------------

	[BoxGroup("$name/Split/Left/Effectors")]

	// Volume Type
	[BoxGroup("$name/Split/Left/Effectors")]
	[ValueDropdown("VolumeType")]
	[LabelWidth(130)]
	public bool volumeType;

	// Loop
	[BoxGroup("$name/Split/Left/Effectors")]
	[LabelWidth(130)]
	public bool loop = false;

	// -------------
	//   Start And Stop Times
	// -------------

	[BoxGroup("$name/Split/Left/Effectors")]

	// Start At
	[BoxGroup("$name/Split/Left/Effectors")]
	[LabelWidth(130)]
	public float StartAt = 0;

	// Has Stop Time
	[BoxGroup("$name/Split/Left/Effectors")]
	[LabelWidth(130)]
	public bool hasStopTime = false;

	// Stop At
	[BoxGroup("$name/Split/Left/Effectors")]
	[ShowIf("hasStopTime")]
	[LabelWidth(130)]
	public float StopAt = 0;



	// -------------
	//	  Pitch
	// -------------

	[BoxGroup("$name/Split/Right/Pitch")]

	// Pitch
	[BoxGroup("$name/Split/Right/Pitch")]
	[LabelWidth(130)]
	[Range(0f, 1f)]
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
	[Range(0f, 1f)]
	public float volume = .75f;


	// Volume Variance
	[VerticalGroup("$name/Split/Right")]
	[BoxGroup("$name/Split/Right/Volume")]
	[LabelWidth(130)]
	[Range(0f, 1f)]
	public float volumeVariance = .1f;


	// -------------
	//    Queues
	// -------------

	[VerticalGroup("$name/Split/Right")]
	[BoxGroup("$name/Split/Right/Queues")]

	// Scene Queues
	[VerticalGroup("$name/Split/Right")]
	[BoxGroup("$name/Split/Right/Queues")]
	[LabelWidth(130)]
	public string SceneQueue = "";

	// Scene End Queues
	[VerticalGroup("$name/Split/Right")]
	[BoxGroup("$name/Split/Right/Queues")]
	[LabelWidth(130)]
	public string EndSceneQueue = "";

	// Scene Queues Transition Time
	[VerticalGroup("$name/Split/Right")]
	[BoxGroup("$name/Split/Right/Queues")]
	[LabelWidth(190)]
	public float SceneQueueTransitionTime = 1;


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

	// Transition Target Volume
	[ShowIf("HasTransition")]
	[HorizontalGroup("$name/Split2")]
	[VerticalGroup("$name/Split2/Left2")]
	[BoxGroup("$name/Split2/Left2/Transition Clip")]
	[LabelWidth(160)]
	public float transitionTargetVolume;


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
	public float transitionAfter = 0;

	[ShowIf("HasTransition")]
	[BoxGroup("$name/Split2/Right2/Transition After")]
	[LabelWidth(130)]
	[ReadOnly]
	[HideLabel]
	[TextArea]
	public string transition = "Transition to next sound after ___ seconds. If left at 0, time will be set the the length of the clip";


	#endregion





	// -------------
	//    Hidden
	// -------------

	[HideInInspector]
	public AudioSource source;

	// -------------
	//    Other
	// -------------

	private static IEnumerable VolumeType = new ValueDropdownList<bool>()
	{
	{ "Music", true },
	{ "Sound Effect", false },
	};

}







