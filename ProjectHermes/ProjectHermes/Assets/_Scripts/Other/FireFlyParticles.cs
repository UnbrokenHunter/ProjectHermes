using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class FireFlyParticles : MonoBehaviour
    {

		#region Variables

		[SerializeField] private ParticleSystem _start;
		[SerializeField] private ParticleSystem _end;


		#endregion


		#region Methods

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.gameObject.tag == "Player")
			{
				_start.Stop();
				_end.gameObject.SetActive(true);
			}
		}

		#endregion

	}
}
