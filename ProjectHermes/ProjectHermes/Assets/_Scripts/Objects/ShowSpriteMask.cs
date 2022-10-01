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

		private Vector2 location;
		private bool isTriggered = false;

		#endregion

		#region User Variables 

		[SerializeField] private float revealSpeed = 1;

		#endregion

		#region Unity Methods

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.tag == "Player" && !isTriggered)
			{
				location = transform.parent.position;

				isTriggered = true;

				transform.parent.position = new Vector3(transform.parent.position.x - transform.parent.lossyScale.x, transform.parent.position.y, transform.parent.position.z);

				gameObject.GetComponentInParent<SpriteMask>().enabled = true;
			}
		}

		private void Update()
		{
			if(isTriggered)
			{
				transform.parent.position = Vector3.Lerp(transform.parent.position, location, revealSpeed * Time.deltaTime);
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			Gizmos.DrawWireCube(transform.parent.position, transform.parent.localScale);

			Gizmos.color = Color.green;
			Gizmos.DrawCube(transform.parent.position, new Vector3(1, 1, 1));
		}

		#endregion

		#region User Methods



		#endregion

	}
}
