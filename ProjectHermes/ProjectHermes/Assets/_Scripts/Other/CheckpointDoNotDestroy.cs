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

		#endregion

		#region Unity Methods

		private void OnLevelWasLoaded(int level)
		{
			if(level == 0)
			{
				hasCheckpoint = false;
			}
		}

		#endregion
	}
}
