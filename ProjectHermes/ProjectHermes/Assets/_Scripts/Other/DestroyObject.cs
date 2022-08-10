using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class DestroyObject : MonoBehaviour
    {

    	#region Methods

        public void DestroyGameobject()
		{
            Destroy(gameObject);
		}

    	#endregion

    }
}
