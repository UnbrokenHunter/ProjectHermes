using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class PlaySound : MonoBehaviour
    {

    	#region Variables
        
        [SerializeField] private string nameOfSound= "Enter the name of the sound to play";

    	#endregion


    	#region Methods

        private void PlaySong()
		{
            AudioManager.instance.Play(nameOfSound);
        }

    	#endregion

    }
}
