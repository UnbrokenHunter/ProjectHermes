using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class FireballScript : MonoBehaviour
    {

        #region Variables

        private Rigidbody2D rb;

        private Vector2 direction;
        private float angle;
        private float hypotenuse;

        #endregion

        #region Methods

        private void Awake()
        {
            rb = GetComponentInParent<Rigidbody2D>();

        }

        private void Update()
        {

            direction = rb.velocity;

            hypotenuse = Mathf.Sqrt((direction.x * direction.x) + (direction.y * direction.y));

            angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Apply the angly we calculated

        }

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if(collision.gameObject.layer == 3)
			{
                AudioManager.instance.Play("FireBounce");
			}
		}

		#endregion

	}
}
