using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TarodevController;

namespace ProjectHermes
{
    public class EnemyCollision : KillPlayer
    {

        #region Variables

        [SerializeField] private float _killBounceAmount = 1;

        [SerializeField] private Animator _anim;

        #endregion

    	#region Methods

        void Awake()
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }
        }

		// Kill player
		private void OnTriggerEnter2D(Collider2D collision)
		{
            OnPlayerDeath(collision.gameObject);
            
            if (collision.gameObject.layer == 6 && collision.gameObject.tag != "Player")
            {
                Destroy(collision.gameObject);
                _anim.Play("Death");

            }
        }

        // Kill monster
		private void OnCollisionEnter2D(Collision2D other)
		{
			if(other.gameObject.tag == "Player")
			{

                AudioManager.instance.Play("KillEnemy");

                // Bounce Player
                other.gameObject.GetComponent<PlayerController>().ApplyVelocity(transform.up.normalized * _killBounceAmount, PlayerForce.Decay);

                // Death Animation
                _anim.Play("Death");

       		}

            else if (other.gameObject.layer == 6)
            {

                AudioManager.instance.Play("KillEnemy");
                Destroy(other.gameObject);
                _anim.Play("Death");

            }
        }

        private void destroyObject()
		{
            if(this.gameObject != null) Destroy(this.gameObject);
		}

        private void disableColliders()
		{
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}

		#endregion

	}
}
