using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class BlockEnemyCollision : MonoBehaviour
    {

		#region Variables

		[SerializeField] private List<GameObject> enemyCollision;

		#endregion

		#region Methods

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.tag == "Enemy")
			{
				enemyCollision.Add(other.gameObject);

			}

		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if(other.gameObject.tag == "Enemy")
			{
				enemyCollision.Remove(other.gameObject);

			}

		}

		public void KillEnemy()
		{
			try
			{
				enemyCollision[0].GetComponent<EnemyCollision>().KillMonster();
			}
			catch(ArgumentOutOfRangeException e)
			{

			}
		}

		#endregion

	}
}
