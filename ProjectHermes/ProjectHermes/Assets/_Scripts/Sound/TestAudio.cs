using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class TestAudio : MonoBehaviour
    {

        [Title("Clip Settings")]
        [SerializeField] [LabelWidth(130)] private string soundName;
        [SerializeField] [LabelWidth(130)] [Range(0, 2)] private float fadeDuration = 1;
        [SerializeField] [LabelWidth(130)] [Range(0, 1)] private float volumeTarget = 1;
    	
        [Title("Actions")]

        [Button]
        private void StartSound()
		{
            AudioManager.instance.Play(soundName);
		}

        [Button]
        private void StopSound()
		{
            AudioManager.instance.Stop(soundName);
        }

        [Button]
        private void PauseSound()
        {
            AudioManager.instance.Pause(soundName);
        }
        
        [Button]
        private void UnpauseSound()
        {
            AudioManager.instance.Unpause(soundName);
        }

        [Button]
        private void FadeInSound()
		{
            AudioManager.instance.FadeInVolume(soundName, fadeDuration);
		}

        [Button]
        private void FadeOutSound()
		{
            AudioManager.instance.FadeOutVolume(soundName, fadeDuration);
        }



    }
}
