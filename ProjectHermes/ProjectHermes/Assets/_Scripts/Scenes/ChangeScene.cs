using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectHermes
{
    public class ChangeScene : MonoBehaviour
    {

        #region Variables

        [SerializeField] private Animator transition;
        [SerializeField] private float transitionLength = 1f;

        public static ChangeScene instance;

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

		public void LoadNextLevel()
		{
            StartCoroutine(LoadLevel(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).buildIndex));
		}

		public void ReloadScene()
		{
			StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
		}

        public void LoadByIndex(int index)
		{
            StartCoroutine(LoadLevel(index));

		}

        public void LoadByString(string name)
		{
            StartCoroutine(LoadLevel(name));
		}

		private IEnumerator LoadLevel(int index)
		{
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionLength);

			print(index);

            SceneManager.LoadScene(index);

		}

		private IEnumerator LoadLevel(string name)
		{
			transition.SetTrigger("Start");

			yield return new WaitForSeconds(transitionLength);

			print(name);

			SceneManager.LoadScene(name);

		}

		private void OnLevelWasLoaded(int level)
		{
			transition.SetTrigger("End");
		}

		#endregion

	}
}
