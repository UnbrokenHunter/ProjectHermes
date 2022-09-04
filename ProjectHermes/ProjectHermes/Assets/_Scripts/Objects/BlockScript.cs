using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class BlockScript : MonoBehaviour
    {

		#region Variables

		[HideIf("@BlockHitbox != null")]
		[SerializeField] private GameObject BlockHitbox;

		[HideIf("@BlockHitbox != null")]
		[SerializeField] private Animator anim;

		[Title("Prefab Settings")]
		[SerializeField] private GameObject fishPrefab;
		[SerializeField] private GameObject rewardPrefab;

		[Title("Block Settings")]
		[SerializeField] private bool destroyOnBreak = false;

		[Title("Reward Settings")]
		[SerializeField] private bool hasFish;
		[SerializeField] private bool hasPowerup;
		[SerializeField] private float animationLength;
		[SerializeField] private float fishAnimationLength;

		private bool isSolid = false;

		#endregion

		#region Methods

		private void OnCollisionEnter2D(Collision2D other)
		{
			if(other.gameObject.tag == "Player" && !isSolid)
			{
				AudioManager.instance.Play("BreakBlock");
				anim.SetTrigger("Break");

				// If block has fish, spawn fish and do its animation
				if(hasFish)
				{
					GameObject fish = Instantiate(fishPrefab, transform);
					fish.transform.localPosition = new Vector3(0, 0, 0);
					fish.transform.localScale = Vector3.one;
					fish.GetComponentInChildren<SpriteRenderer>().sortingOrder = this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder - 1;
					fish.GetComponent<FishScript>().FishFromBlock();
					fish.GetComponent<BoxCollider2D>().enabled = false;
				}

				// If has powerup, spawn powerup
				if(hasPowerup)
				{
					GameObject fish = Instantiate(rewardPrefab, transform);
					fish.transform.localPosition = new Vector3(0, 1, 0);
					fish.GetComponentInChildren<SpriteRenderer>().sortingOrder = this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder - 1;

				}

				StartCoroutine(DestroyBlock());

			}
		}

		private IEnumerator DestroyBlock()
		{
			yield return new WaitForSeconds(animationLength);
			if (destroyOnBreak)
			{
				if(hasFish)
				{
					yield return new WaitForSeconds(fishAnimationLength);
				}
				Destroy(this.gameObject);

			}
			else
			{
				isSolid = true;
				anim.SetTrigger("Solid");
			}
		}

		#endregion

	}
}
