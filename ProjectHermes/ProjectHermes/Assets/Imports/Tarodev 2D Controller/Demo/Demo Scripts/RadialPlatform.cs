using UnityEngine;

namespace TarodevController {
    public class RadialPlatform : PlatformBase {
        [SerializeField] private float _speed = 3;
        [SerializeField] private float _size = 2;

        private Transform _t;
        private Vector3 _startPos;
        private Vector3 _lastPos;

        private Vector3 Pos => _t.position;

        private void Awake() {
            _t = transform;
            _startPos = Pos;
        }

        private void Update() {
            _t.position = _startPos + new Vector3(Mathf.Cos(Time.time * _speed), Mathf.Sin(Time.time * _speed)) * _size;
        }

        private void FixedUpdate() {
            var change = _lastPos - Pos;

            _lastPos = Pos;

            MovePlayer(change);
        }


        private void OnDrawGizmosSelected() {
            if (Application.isPlaying) return;
            Gizmos.DrawWireSphere(transform.position, _size);
        }
    }
}