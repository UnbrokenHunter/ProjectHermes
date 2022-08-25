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

        private BoxCollider2D[] _cols;
        private GameObject _player;

        #endregion

        #region User Variables 

        [SerializeField] private float _lerpSpeed = 5;
        [SerializeField] private float yOffset;
        [SerializeField] private float _lerpThreshold = .1f;

        [Header("Location")]
        [SerializeField] private GameObject _exitPipe;

		#endregion

		#region Unity Methods

		private void Awake()
		{
            _cols = this.gameObject.GetComponents<BoxCollider2D>();
            _player = GameObject.Find("Player");
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            DownPipe(other);
		}


        #endregion

        #region User Methods

        private void DownPipe(Collider2D other)
		{
            if (other.gameObject.tag == "Player")
            {
                if (other.gameObject.GetComponent<PlayerController>()._goPipe)
                {
                    other.gameObject.GetComponent<PlayerController>().MovePlayerToPipe(this.transform, _lerpSpeed);

                    if (this.transform.position.x - _lerpThreshold <= other.transform.position.x && other.transform.position.x <= this.transform.position.x + _lerpThreshold) 
                    {
                        other.gameObject.GetComponentInChildren<PlayerAnimator>().PlayerEnterPipe();

                        other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                        other.gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;

                        other.GetComponentInChildren<PlayerAnimator>()._nextPipe = _exitPipe;
                    }
                }
            }
        }

        public void UpPipe()
		{
            print("Up Pipe");

            // Teleport player to pipe
            _player.transform.position = this.transform.position + new Vector3(0, yOffset, 0);

        }

        #endregion


    }
}
