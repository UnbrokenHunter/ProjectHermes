using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TarodevController;

namespace ProjectHermes
{
    public class RotatingPlatform : PlatformBase
    {

        #region Variables

        [SerializeField] private Vector3 _rotationSpeed = Vector3.back;
        [SerializeField] private float _speed = 2;
        [SerializeField] private float _size = 2;

        private CircleCollider2D _col;
        private Transform _t;
        private Vector3 _startRot;
        private Vector3 _lastRot;
        private Vector3 _lastPos;
        private Transform otherPosition;

        private Vector3 Rot => otherPosition.rotation.eulerAngles;

        #endregion

        #region Methods

        private void Awake()
        {
            _col = GetComponent<CircleCollider2D>();

            _t = transform;
        }

        private void Update()
        {
            _t.Rotate(_rotationSpeed * Time.deltaTime);
            
        }

        private void FixedUpdate()
        {

            if (otherPosition != null)
            {
                otherPosition.parent = this.transform;

                //rint(change);

                //MovePlayer(change);

            }
        }

		private void OnCollisionStay2D(Collision2D collision)
		{
            if (collision.gameObject.tag == "Player")
            {
                otherPosition = collision.transform;
                otherPosition.RotateAround(this.gameObject.transform.position, Vector3.forward, _speed * Time.deltaTime);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            otherPosition = null;
            collision.transform.parent = null;
        }

        #endregion

    }
}
