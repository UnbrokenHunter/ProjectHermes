using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class FlyingSpikeScript : KillPlayer
    {

		#region Variables

		public float lifespan = 5;

		#endregion

		#region Methods

		private IEnumerator Start()
		{
			yield return new WaitForSeconds(lifespan);

			Destroy(gameObject);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if(collision.gameObject.tag == "Player")
			{
				OnPlayerDeath(collision.gameObject);
				Destroy(gameObject);
			}
		}

		#endregion

	}
}
