using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectHermes
{
    public class CheckpointDoNotDestroy : MonoBehaviour
    {
		#region Variables 

		public bool hasCheckpoint = false;
		public int coinCount;
		public int timerCount;

		#endregion

		#region Unity Methods

		private void OnLevelWasLoaded(int level)
		{
			if(level == 0)
			{
				hasCheckpoint = false;
				coinCount = 0;
			}
		}

		#endregion
	}
}
