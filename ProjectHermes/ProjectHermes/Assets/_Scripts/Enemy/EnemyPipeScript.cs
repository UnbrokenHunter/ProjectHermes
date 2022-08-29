using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class EnemyPipeScript : MonoBehaviour
    {

        #region Variables

        [HideInInspector] public bool inPipe = false;

        [HideInInspector] public Transform spawn;
        [HideInInspector] public Transform exit;
        [HideInInspector] public float speed;

        private AIPatrol patrol;
        private Rigidbody2D rb;
        private Collider2D[] cols;

		#endregion

		#region Methods

		private void Awake()
		{
            patrol = gameObject.GetComponent<AIPatrol>();
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        public void DisableColliders()
		{
            rb.bodyType = RigidbodyType2D.Kinematic;
            cols = this.gameObject.GetComponentsInChildren<Collider2D>();

            foreach (Collider2D col in cols)
			{
                col.enabled = false;
			}
		}

        private void EnableColliders()
		{
            rb.bodyType = RigidbodyType2D.Dynamic;
            foreach (Collider2D col in cols)
            {
                col.enabled = true;
            }
        }

		private void Update()
        {
            if (!inPipe) return;

            transform.position = Vector2.MoveTowards(transform.position, exit.position, speed);

            patrol.mustPatrol = false;

            if (transform.position == exit.position)
            {
                EnableColliders();
                patrol.mustPatrol = true;
                inPipe = false;
            }
        }

        #endregion

    }
}
