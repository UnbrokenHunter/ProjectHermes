using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class EnemyCollisionBoundary : MonoBehaviour
    {
		#region Program Variables



		#endregion

		#region User Variables 



		#endregion

		#region Unity Methods

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawCube(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.extents * 2);
		}

		#endregion

	}
}
