using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace ProjectHermes
{
    public class EndLevel : MonoBehaviour
    {

		#region Program Variables



		#endregion

		#region User Variables 

		[SerializeField] private PlayableDirector Timeline;
		[SerializeField] private CinemachineVirtualCamera endLevelCam;

		#endregion

		#region Unity Methods

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag != "Player") return;

			endLevelCam.Priority = 11;
			Timeline.Play();

		}

		#endregion

	}
}
