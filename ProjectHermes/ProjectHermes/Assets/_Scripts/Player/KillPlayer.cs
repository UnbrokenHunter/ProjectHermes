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

        protected void OnPlayerDeath(GameObject player)
		{
            if (player.tag == "Player")
            {
                // If player has star, dont kill
                if(player.GetComponent<StarController>().hasStarEffect)
				{
                    if(this.gameObject.tag == "Enemy")
					{
                        AudioManager.instance.Play("KillEnemy");
                        Destroy(this.gameObject);
					}
				}
                // If player has fire, demote but dont kill
                else if (player.GetComponent<FireballController>().isFireUpgraded)
                {
					player.GetComponent<FireballController>().isFireUpgraded = false;

                }
                // if player has nothing, kill
                else if (!player.GetComponent<FireballController>().isInvincible)
                {
                    AudioManager.instance.Play("PlayerDeath");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        protected void OnPlayerDeath(GameObject player, bool isVoid)
        {
            if(isVoid == true && player.gameObject.tag == "Player")
            {
				AudioManager.instance.Play("PlayerDeath");
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
            else if (isVoid == true)
            {
                Destroy(player.gameObject);
            }
        }

		#endregion

	}
}
