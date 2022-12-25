using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class CheckpointScript : MonoBehaviour
    {

		#region Unity Methods

		private void Start()
		{
			if (DoNotDestroy.instance.GetComponentInChildren<CheckpointDoNotDestroy>().hasCheckpoint == true)
			{
				GameObject.Find("Player").transform.position = transform.position;
				
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag != "Player") return;
			//if (DoNotDestroy.instance.GetComponentInChildren<CheckpointDoNotDestroy>().hasCheckpoint == true) return;

			AudioManager.instance.Play("Checkpoint");
			GetComponentInChildren<Animator>().SetTrigger("FlagHit");

			DoNotDestroy.instance.GetComponentInChildren<CheckpointDoNotDestroy>().hasCheckpoint = true;
		}

		#endregion

	}
}
