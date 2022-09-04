using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TarodevController;
using UnityEngine.Tilemaps;
using System.Threading;

namespace ProjectHermes
{
    public class FallingBlockScript : PlatformBase
    {
        #region Program Variables

        private Rigidbody2D rb;
        private bool startFall = false;
        private float waitTimer = 0;

        #endregion

        #region User Variables 

        [SerializeField] private float fallDelay;
        [SerializeField] private float fallSpeed;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.tag == "Player")
            {
                startFall = true;
            }
        }

        private void Update()
        {
            if(startFall)
            {
                waitTimer += Time.deltaTime;

                // If the counter is bigger than our delay, begin moving downwards
                if (waitTimer > fallDelay)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

                    if (__player != null) MovePlayer(Vector2.down * fallSpeed * Time.deltaTime);

                    rb.velocity = Vector2.down * fallSpeed * Time.deltaTime;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }                       
        }

        #endregion
    }
}
