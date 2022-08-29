using UnityEngine;

namespace TarodevController {
    public class PlatformBase : MonoBehaviour {
        [Tooltip("If velocity is above this threshold the platform will not affect the player")] [SerializeField]
        private float _unlockThreshold = 2.5f;

        protected Rigidbody2D __player;

        private void OnCollisionEnter2D(Collision2D col) {
            if (col.transform.TryGetComponent(out IPlayerController controller)) __player = col.transform.GetComponent<Rigidbody2D>();
        }

        private void OnCollisionExit2D(Collision2D col) {
            if (col.transform.TryGetComponent(out IPlayerController controller)) __player = null;
        }

        protected void MovePlayer(Vector2 change) {
            if (__player) {
                // This prevents jumping and moving from being locked
                if (__player.velocity.magnitude >= _unlockThreshold) return;
                __player.MovePosition(__player.position - change);                
            }
        }
    }
}