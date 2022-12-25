using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class MushroomScript : MonoBehaviour
    {

        #region Variables

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private bool flip;
        [SerializeField] private float bulletSpeed = 5;
        [SerializeField] private float bulletLifespan = 5;
        [SerializeField] private float shootingRate = 1;
        [SerializeField] private GameObject fireSpot;
        [SerializeField] private float loadingDistance = 30;
        [SerializeField] private float invincableTime = 0.5f;
        private Animator anim;

		#endregion


		#region Methods

		private void Awake()
		{
            anim = GetComponent<Animator>();

			if (flip)
			{
                transform.localScale = new Vector3(-1, 1, 1);
			}

            InvokeRepeating("TriggerShot", 1, shootingRate);

		}

		private void TriggerShot()
		{
            anim.SetTrigger("shoot");
        }

        public void ShootBullet()
        {
            if (Vector2.Distance(GameObject.Find("Player").transform.position, this.gameObject.transform.position) < loadingDistance)
            {

                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = fireSpot.transform.position;
                bullet.GetComponent<BulletScript>().bulletSpeed = bulletSpeed;
                bullet.GetComponent<BulletScript>().bulletLifespan = bulletLifespan;
                bullet.GetComponent<BulletScript>().invinsableTime = invincableTime;

                AudioManager.instance.Play("ShootBullet");

                if (gameObject.transform.lossyScale.x > 0)
                {
                    bullet.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    bullet.GetComponent<BulletScript>().left = true;
                }

            }
        }

    	#endregion

    }
}
