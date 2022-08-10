using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class PauseUI : MonoBehaviour
    {

        #region Variables

        public Object[] UIElements;

        [SerializeField] private GameObject PauseMenuCanvas;
        [SerializeField] private GameObject PauseMenuButtons;
        [SerializeField] private GameObject SettingsMenuButtons;

        #endregion


        #region Methods

        #region Pause
        public void PauseGame()
		{
            // Stop the game
            Time.timeScale = 0;

            // Find and Disable all other UI
            FindUIElements();
            foreach(GameObject go in UIElements)
			{
                go.SetActive(false);                
			}

            // Enable This UI
            PauseMenuCanvas.SetActive(true);
            PauseMenuButtons.SetActive(true);
            SettingsMenuButtons.SetActive(false);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void UnpauseGame()
		{
            // Un-Stop the Game
            Time.timeScale = 1;

            // Reenable previous UI Elements
            foreach(GameObject go in UIElements)
			{
                go.SetActive(true);
			}

            // Disable this UI
            PauseMenuCanvas.SetActive(false);
            PauseMenuButtons.SetActive(false);
            SettingsMenuButtons.SetActive(false);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

		#endregion

		#region Settings

        public void EnableSettingsMenu()
		{
            // Make Sure background is enabled
            PauseMenuCanvas.SetActive(true);

            // Disable Pause Buttons
            PauseMenuButtons.SetActive(false);

            // Enable Settings Buttons
            SettingsMenuButtons.SetActive(true);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void DisableSettingsMenu()
        {
            // Make Sure background is enabled
            PauseMenuCanvas.SetActive(true);

            // Disable Pause Buttons
            PauseMenuButtons.SetActive(true);

            // Enable Settings Buttons
            SettingsMenuButtons.SetActive(false);

            // Play Audio
            AudioManager.instance.Play("Click1");

        }

        #endregion

        #region Menu

        public void BackToMenu()
		{
            UnpauseGame();

            // Play Audio
            AudioManager.instance.Play("Click1");

            ChangeScene.instance.LoadByIndex(0);
		}

        #endregion

		private void FindUIElements()
		{
            UIElements = GameObject.FindGameObjectsWithTag("UI");
		}

    	#endregion

    }
}
