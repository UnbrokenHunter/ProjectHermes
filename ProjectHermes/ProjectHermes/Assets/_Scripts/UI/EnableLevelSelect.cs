using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class EnableLevelSelect : MonoBehaviour
    {

        #region Variables

        [SerializeField] private GameObject levelSelect;
        [SerializeField] private GameObject titleScreen;

        [SerializeField] private string levelOne;
        [SerializeField] private string levelTwo;
        [SerializeField] private string levelThree;
        [SerializeField] private string levelFour;
        [SerializeField] private string levelFive;
        [SerializeField] private string levelSix;
        [SerializeField] private string levelSeven;
        [SerializeField] private string levelEight;
        [SerializeField] private string levelNine;
        [SerializeField] private string levelTen;

        #endregion


        #region Methods

        public void LevelSelect()
		{
            levelSelect.SetActive(true);    
            titleScreen.SetActive(false);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void DisableLevelSelect()
        {
            levelSelect.SetActive(false);
            titleScreen.SetActive(true);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level1()
		{
            ChangeScene.instance.LoadByString(levelOne);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level2()
		{
            ChangeScene.instance.LoadByString(levelTwo);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level3()
		{
            ChangeScene.instance.LoadByString(levelThree);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level4()
		{
            ChangeScene.instance.LoadByString(levelFour);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level5()
		{
            ChangeScene.instance.LoadByString(levelFive);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level6()
		{
            ChangeScene.instance.LoadByString(levelSix);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level7()
		{
            ChangeScene.instance.LoadByString(levelSeven);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level8()
		{
            ChangeScene.instance.LoadByString(levelEight);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level9()
		{
            ChangeScene.instance.LoadByString(levelNine);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        public void Level10()
		{
            ChangeScene.instance.LoadByString(levelTen);

            // Play Audio
            AudioManager.instance.Play("Click1");
        }

        #endregion

    }
}
