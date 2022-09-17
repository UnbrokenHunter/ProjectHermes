using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectHermes
{
    public class ShowSpriteMask : MonoBehaviour
    {
		#region Program Variables

		[HideIf("@box != null")]
		[SerializeField]
		private BoxCollider2D box;


		#endregion

		#region User Variables 


		#endregion

		#region Unity Methods

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.tag == "Player")
			{
				gameObject.GetComponentInParent<SpriteMask>().enabled = true;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			Gizmos.DrawWireCube(transform.parent.position, transform.parent.localScale);
		}

		#endregion

		#region User Methods



		#endregion

	}
}
