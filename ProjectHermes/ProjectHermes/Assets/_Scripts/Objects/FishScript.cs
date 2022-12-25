using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class FishScript : MonoBehaviour
    {

		#region Variables

		[SerializeField] private LevelManager levelManager;
		private Animator animator;
		[SerializeField] protected float animationLength;

		#endregion

		#region Methods

		private void Awake()
		{
			animator = GetComponentInChildren<Animator>();
			if(levelManager == null)
			{
				levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == "Player")
			{
				AddtoCount();     
				AudioManager.instance.Play("PickupItem");
				this.gameObject.SetActive(false);
			}
		}

		public void FishFromBlock()
		{
			AddtoCount();         
			AudioManager.instance.Play("PickupItem");
			animator.SetTrigger("FromBlock");
			StartCoroutine(DestroyObject());
		}

		private void AddtoCount()
		{
			levelManager.IncreaseCoinCount();
		}

		private IEnumerator DestroyObject()
		{
			yield return new WaitForSeconds(animationLength);
			this.gameObject.SetActive(false);

		}

		#endregion

	}
}
