using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TarodevController;
using UnityEngine;

namespace ProjectHermes
{
    public class PipeScript : MonoBehaviour
    {

        #region Program Variables

        #endregion

        #region User Variables 

        [SerializeField] private float _lerpSpeed = 5;

        [Header("Location")]
        [SerializeField] private GameObject _exitPipe; 

        #endregion

        #region Unity Methods

        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.gameObject.tag == "Player")
            {
                if(other.gameObject.GetComponent<PlayerController>()._goPipe)
                {
					other.gameObject.GetComponent<PlayerController>().MovePlayerToPipe(this.transform, _lerpSpeed);
					other.gameObject.GetComponent<PlayerAnimator>().PlayerEnterPipe();
                }
            }
		}


        #endregion

        #region User Methods



		#endregion


	}
}
