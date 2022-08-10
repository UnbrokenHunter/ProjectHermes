using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectHermes
{
    public class KillPlayer : MonoBehaviour
    {

    	#region Methods

        protected void OnPlayerDeath(GameObject other)
		{
            if (other.tag == "Player")
            {
                if(other.GetComponent<StarController>().hasStarEffect)
				{

				}
                else if (other.GetComponent<FireballController>().isFireUpgraded)
                {
                    other.GetComponent<FireballController>().isFireUpgraded = false;

                }
                else if (!other.GetComponent<FireballController>().isInvincible)
                {
                    AudioManager.instance.Play("PlayerDeath");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

    	#endregion

    }
}
