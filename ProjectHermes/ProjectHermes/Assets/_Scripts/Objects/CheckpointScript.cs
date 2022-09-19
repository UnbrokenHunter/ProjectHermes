using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class CheckpointScript : MonoBehaviour
    {
		#region User Variables 



		#endregion

		#region Unity Methods

		private void Awake()
		{
			if(DoNotDestroy.instance.GetComponentInChildren<CheckpointDoNotDestroy>().hasCheckpoint == true)
			{
				GameObject.Find("Player").transform.position = transform.position;
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			DoNotDestroy.instance.GetComponentInChildren<CheckpointDoNotDestroy>().hasCheckpoint = true;

			GetComponentInChildren<Animator>().SetTrigger("FlagHit");

			// AudioManager.instance.Play()
		}

		#endregion

	}
}
