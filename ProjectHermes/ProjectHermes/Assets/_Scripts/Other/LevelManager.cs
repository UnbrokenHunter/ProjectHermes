using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ProjectHermes
{
    public class LevelManager : MonoBehaviour
    {

		#region Coin Count

		#region Variables

		[Title("Coin Count")]
		public int coinCount = 0;
        [SerializeField] private TMPro.TMP_Text text;
    	#endregion

    	#region Methods

        public void IncreaseCoinCount()
		{
            coinCount++;

            text.text = coinCount.ToString() + "x";
		}

		#endregion

		#endregion

		#region Star Coin

		[Title("Star Coin")]
		[SerializeField] private Image[] starCoin;
		[SerializeField] private Color StarEmptyColor;
		[SerializeField] private int starCoinsCollected = 0;

		public void StarCoinCollected()
		{
			starCoin[starCoinsCollected].color = StarEmptyColor;
			starCoinsCollected++;
			print("Sprite Changed");
		}

		#endregion

		#region Timer 

		[Title("Timer")]
		[SerializeField] private string timerPretext;
		[SerializeField] private TMP_Text timer;
		private float timeTracker = 0;
		public int timeCounter = 0;

		private void Update()
		{
			timeTracker += Time.deltaTime;

			if(timeTracker > 1)
			{
				timeTracker--;
				timeCounter++;
				timer.text = timerPretext + timeCounter + "";
			}
		}


		#endregion

	}
}
