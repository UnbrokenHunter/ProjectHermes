using UnityEngine;

namespace TarodevController {
    public class PatrolPlatform : PlatformBase {
        [SerializeField] private Vector2[] _points;
        [SerializeField] private float _speed = 1;
        [SerializeField] private bool _looped;


        private Rigidbody2D _rb;
        private Vector2 _startPos;
        private int _index;
        private Vector2 Pos => _rb.position;
        private Vector2 _lastPos;
        private bool _ascending;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _startPos = _rb.position;
        }

        private void FixedUpdate() {
            var target = _points[_index] + _startPos;
            var newPos = Vector2.MoveTowards(Pos, target, _speed * Time.fixedDeltaTime);
            _rb.MovePosition(newPos);

            if (Pos == target) {
                _index = _ascending ? _index + 1 : _index - 1;
                if (_index >= _points.Length) {
                    if (_looped) {
                        _index = 0;
                    }
                    else {
                        _ascending = false;
                        _index--;
                    }
                }
                else if (_index < 0) {
                    _ascending = true;
                    _index = 1;
                }
            }

            var change = _lastPos - newPos;
            _lastPos = newPos;

            MovePlayer(change);
        }

        private void OnDrawGizmosSelected() {
            if (Application.isPlaying) return;
            var curPos = (Vector2)transform.position;
            var previous = curPos + _points[0];
            for (var i = 0; i < _points.Length; i++) {
                var p = _points[i] + curPos;
                Gizmos.DrawWireSphere(p, 0.2f);
                Gizmos.DrawLine(previous, p);

                previous = p;

                if (_looped && i == _points.Length - 1) Gizmos.DrawLine(p, curPos + _points[0]);
            }
        }
    }
}