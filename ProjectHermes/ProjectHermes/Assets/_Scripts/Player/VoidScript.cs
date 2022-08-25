using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class VoidScript : KillPlayer
    {

		[SerializeField] private GameObject _player;

		#region Unity Methods
		
		private void FixedUpdate()
		{
			transform.position = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			OnPlayerDeath(collision.gameObject, true);
		}

		#endregion

	}
}
