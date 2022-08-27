using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class BallThrowerScript : MonoBehaviour
    {
        #region Program Variables

        private AIPatrol patrol;
		private float timer = 0;



		#endregion

		#region User Variables 

		[SerializeField] private GameObject ballPrefab;
        [SerializeField] private Vector2 throwSpeed;
        public bool throwBall;

        [SerializeField] private float throwInterval;

        [SerializeField] private float _waitAfterThrow = 5;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            patrol = gameObject.GetComponent<AIPatrol>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > _waitAfterThrow)
            {
                throwBall = true;
            }

            if (throwBall)
            {
                patrol.mustPatrol = false;
                StartCoroutine(ThrowBall());

                timer = 0;
            }
        }

        #endregion

        #region User Methods

        private IEnumerator ThrowBall()
        {
            GameObject ball = Instantiate(ballPrefab, this.transform);
            if (!gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                ball.GetComponent<SpriteRenderer>().flipX = true;
                throwSpeed.x *= -1;
            }

            ball.gameObject.GetComponent<Rigidbody2D>().velocity = throwSpeed;
            throwBall = false;

            yield return new WaitForSeconds(_waitAfterThrow);

            patrol.mustPatrol = true;
		}
		
    	#endregion 

    }
}
