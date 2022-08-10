using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class DoNotDestroy : MonoBehaviour
    {

        #region Variables

        public static DoNotDestroy instance;

        #endregion


        #region Methods

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

    	#endregion

    }
}
