using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace ProjectHermes
{
    public class EndLevelMenuScript : MonoBehaviour
    {
        #region Program Variables



        #endregion

        #region User Variables 

        [Title("Assets")]
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private TMP_Text _coinText;
        [SerializeField] private TMP_Text _timerText;

        [Title("Variables")]
        [SerializeField] private string _coinPreText;
		[SerializeField] private string _coinPostText;

        [SerializeField] private string _timerPreText;
		[SerializeField] private string _timerPostText;


		#endregion

		#region Unity Methods

		private void Awake()
    	{
            _coinText.text = _coinPreText + levelManager.coinCount + _coinPostText;
            _timerText.text = _timerPreText + levelManager.timeCounter + _timerPostText;
    	}
	
    	#endregion 
	
    	#region User Methods
		
        public void ReturnMainMenu()
        {
            DoNotDestroy.instance.GetComponentInChildren<ChangeScene>().LoadByString("TitleScreen");
        }

    	#endregion 

    }
}
