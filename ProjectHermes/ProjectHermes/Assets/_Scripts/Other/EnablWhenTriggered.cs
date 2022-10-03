using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class EnablWhenTriggered : MonoBehaviour
    {

    	#region Variables
        
        public GameObject ObjectToEnable;

		#endregion

		#region Methods

		private void OnTriggerEnter2D(Collider2D collision)
		{
			ObjectToEnable.SetActive(true);
		}

		#endregion

	}
}
