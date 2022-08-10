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

		private void Start()
		{
            StartCoroutine(DestroyBullet());
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

		private void OnTriggerEnter2D(Collider2D collision)
		{
            OnPlayerDeath(collision.gameObject);
            if(collision.gameObject.tag == "Player") Destroy(gameObject);

        }

        #endregion

    }
}
