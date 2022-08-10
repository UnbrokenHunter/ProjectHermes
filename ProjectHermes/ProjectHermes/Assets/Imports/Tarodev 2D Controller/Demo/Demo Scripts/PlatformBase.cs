using UnityEngine;

namespace TarodevController {
    public class PlatformBase : MonoBehaviour {
        [Tooltip("If velocity is above this threshold the platform will not affect the player")] [SerializeField]
        private float _unlockThreshold = 2.5f;

        private Rigidbody2D _player;

        private void OnCollisionEnter2D(Collision2D col) {
            if (col.transform.TryGetComponent(out IPlayerController controller)) _player = col.transform.GetComponent<Rigidbody2D>();
        }

        private void OnCollisionExit2D(Collision2D col) {
            if (col.transform.TryGetComponent(out IPlayerController controller)) _player = null;
        }

        protected void MovePlayer(Vector2 change) {
            if (_player) {
                // This prevents jumping and moving from being locked
                if (_player.velocity.magnitude >= _unlockThreshold) return;
                _player.MovePosition(_player.position - change);
            }
        }
    }
}