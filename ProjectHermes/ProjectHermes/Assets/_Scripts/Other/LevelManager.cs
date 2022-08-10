using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

namespace ProjectHermes
{
    public class LevelManager : MonoBehaviour
    {

        #region Variables

        [SerializeField] private int coinCount = 0;

        [SerializeField] private TMPro.TMP_Text text;
    	#endregion


    	#region Methods

        public void IncreaseCoinCount()
		{
            coinCount++;

            text.text = coinCount.ToString() + "x";
		}

    	#endregion

    }
}
