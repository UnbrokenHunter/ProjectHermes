using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectHermes
{
    public class SpikeScript : KillPlayer
    {

		#region Variables



		#endregion


		#region Methods

		private void OnTriggerEnter2D(Collider2D other)
		{
            OnPlayerDeath(other.gameObject);
        }

		#endregion

	}
}
