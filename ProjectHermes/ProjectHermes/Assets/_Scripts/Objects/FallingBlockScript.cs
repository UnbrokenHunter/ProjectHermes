using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TarodevController;
using UnityEngine.Tilemaps;

namespace ProjectHermes
{
    public class FallingBlockScript : PlatformBase
    {
        #region Variables

        #region Program Variables

        private GameObject player;
        private Rigidbody2D rb;
        private bool used = false;
        private RaycastHit2D hit;
        

        #endregion

        #region User Variables 

        [SerializeField] private float fallDelay;
        [SerializeField] private float fallSpeed;

        #endregion

        #endregion

        #region Methods

        #region Unity Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.tag == "Player")
            {
                player = other.gameObject;
                StartCoroutine(BlockFall());
            }

            var hit = Physics2D.BoxCast(transform.position, gameObject.GetComponent<BoxCollider2D>().size, 0, Vector2.down, 1);


			if (transform.position.y <= hit.point.y)
            {
                rb.bodyType = RigidbodyType2D.Static;
                this.enabled = false;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            player = null;
        }

        private void Update()
        {
            if(rb.bodyType == RigidbodyType2D.Kinematic)
            {
				if (__player != null)
				{
					MovePlayer(Vector2.down * fallSpeed * Time.deltaTime);
				}

				rb.velocity = Vector2.down * fallSpeed * Time.deltaTime;

            }
        }

        #endregion

        #region User Methods

        private IEnumerator BlockFall()
        {
            yield return new WaitForSeconds(fallDelay);

            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;
            rb.useFullKinematicContacts = true;

            //rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            
        }

        #endregion

        #endregion

    }
}
