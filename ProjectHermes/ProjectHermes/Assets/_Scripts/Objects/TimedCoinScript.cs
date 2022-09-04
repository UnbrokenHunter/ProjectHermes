using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class TimedCoinScript : MonoBehaviour
    {
		#region Program Variables

		public TimedCoinGame manager;

		#endregion

		#region Unity Methods

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.tag == "Player")
			{
				manager.CoinCollected();
				this.gameObject.SetActive(false);
			}
		}

		#endregion

	}
}
