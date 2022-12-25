using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class BulletScript : KillPlayer
    {

        #region Variables

        private Rigidbody2D rb;
        public float bulletSpeed;
        public float bulletLifespan = 5;
        public bool left = false;
        public float invinsableTime;

        private bool canHit = false;

    	#endregion


    	#region Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private IEnumerator DestroyBullet()
		{
            yield return new WaitForSeconds(bulletLifespan);

            Destroy(gameObject);
		}

        private IEnumerator InvincableTimer()
		{
            yield return new WaitForSeconds(invinsableTime);

            canHit = true;
		}


        private void Start()
		{
            StartCoroutine(DestroyBullet());
            StartCoroutine(InvincableTimer());
        }

        private void Update()
        {

            if (left)
			{
                rb.velocity = Vector2.left * bulletSpeed;
			}
            else
			{
                rb.velocity = Vector2.right * bulletSpeed;
			}
            
        }

		private void OnCollisionEnter2D(Collision2D collision)
		{
            if (!canHit) return;

            print("Hit");

            if (collision.gameObject.tag == "Player") OnPlayerDeath(collision.gameObject);

            Destroy(gameObject);
        }

        #endregion

    }
}
