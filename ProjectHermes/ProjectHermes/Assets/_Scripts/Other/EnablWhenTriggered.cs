using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class EnablWhenTriggered : MonoBehaviour
    {

		#region Variables

		[SerializeField] private bool collisionInsteadOfTrigger = false;
        public GameObject ObjectToEnable;
		private bool hasBeenEnabled = false;

		#endregion

		#region Methods

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collisionInsteadOfTrigger) return;
			if (hasBeenEnabled) return;
			ObjectToEnable.SetActive(true);
			hasBeenEnabled = true;
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (!collisionInsteadOfTrigger) return;
			if (hasBeenEnabled) return;
			ObjectToEnable.SetActive(true);
			hasBeenEnabled = true;
		}

		#endregion

	}
}
