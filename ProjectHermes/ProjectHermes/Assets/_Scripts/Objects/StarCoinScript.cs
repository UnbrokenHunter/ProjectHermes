using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class StarCoinScript : MonoBehaviour
    {
    	#region Program Variables
		
		private LevelManager levelManager;
		
    	#endregion 

    	#region User Variables 
		
		
		
    	#endregion 

    	#region Unity Methods
	
    	private void Awake()
    	{
			levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.tag == "Player")
			{
				CoinCollected();
			}
		}

		#endregion

		#region User Methods

		private void CoinCollected()
		{
			print("Star Coin Collected");

			levelManager.StarCoinCollected();

			Destroy(gameObject);

			// Animator
			// AudioManager.instance.Play();

		}

		#endregion

	}
}
