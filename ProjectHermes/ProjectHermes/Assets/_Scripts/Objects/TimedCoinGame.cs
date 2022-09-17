using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class TimedCoinGame : MonoBehaviour
    {
		#region Program Variables

		private Transform coinContainer;
		private int coinsCollected = 0;
		private GameObject rewardObject;
		private bool rewardSummoned = false;
		private float coinCounter = 0;
		private RaycastHit2D hit;

		#endregion

		#region User Variables 

		[Title("Coins")]
		[SerializeField] private GameObject coinPrefab;
		[SerializeField] private Vector2[] coins;

		[Space]
		[SerializeField] private float coinTimer = 10;

		[Title("Reward")]
		[SerializeField] private GameObject reward;
		[SerializeField] private bool rewardFall = false;
		[SerializeField] private float rewardFallSpeed = 60;

		[Title("Gizmos")]
		[SerializeField] private float gizmosRadius = 1;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			coinContainer = GameObject.Find("Coin Container").transform;
		}

		private void Update()
		{
			if(coinContainer.childCount > 0)
			{
				coinCounter += Time.deltaTime;

				if (coinCounter >= coinTimer)
				{
					for (int i = 0; i < coins.Length; i++)
					{
						Destroy(coinContainer.GetChild(i).gameObject);
					}
				}
			}

			if(coinsCollected >= coins.Length && !rewardSummoned)
			{
				SummonReward();
			}


			if(rewardObject != null && rewardFall)
			{
				rewardObject.transform.position = Vector2.MoveTowards(rewardObject.transform.position, hit.point, rewardFallSpeed);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			CreateCoins();
		}

		private void OnDrawGizmos()
		{
			if (Application.isPlaying) return;

			Gizmos.color = Color.red;
			foreach (var coin in coins)
			{
				Gizmos.DrawSphere(-transform.InverseTransformPoint(coin), gizmosRadius);
			}

		}

		#endregion

		#region User Methods

		private void CreateCoins()
		{
			gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<BoxCollider2D>().enabled = false;

			foreach (var coinLocation in coins)
			{
				GameObject coin = Instantiate(coinPrefab, -transform.InverseTransformPoint(coinLocation), new Quaternion(0, 0, 0, 0), coinContainer);
				coin.GetComponent<TimedCoinScript>().manager = this;
			}
		}

		public void CoinCollected()
		{
			coinsCollected++;

			//AudioManager.instance.Play
		}

		private void SummonReward()
		{
			rewardObject = Instantiate(reward, this.transform.position, this.transform.rotation, transform);
			hit = Physics2D.Raycast(rewardObject.transform.position, Vector2.down);
			rewardSummoned = true;
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}

		#endregion

	}
}
