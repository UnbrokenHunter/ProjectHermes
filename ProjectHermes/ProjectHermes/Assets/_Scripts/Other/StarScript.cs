using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class StarScript : MonoBehaviour
    {

        #region Variables

        [SerializeField] private float starEffectLength;

		#endregion


		#region Methods

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == "Player")
			{
				other.gameObject.GetComponent<StarController>().EnableStarEffect(starEffectLength);
				print("Pickup Star");
			}
		}

		#endregion

	}
}
