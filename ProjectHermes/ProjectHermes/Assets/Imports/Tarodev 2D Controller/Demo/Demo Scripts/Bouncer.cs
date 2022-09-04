using UnityEngine;

namespace TarodevController
{
	public class Bouncer : MonoBehaviour
	{
		[SerializeField] private float _bounceForce = 70;
		[SerializeField] private float jumpBonus = 5;
		[SerializeField] private float nonJumpMultiplier = 1;

		private float jumpBonusStorage = 1;

		private void OnCollisionStay2D(Collision2D other)
		{

			// IF JUMPING ADD EXTRA
			if (other.gameObject.GetComponent<PlayerController>()._frameInput.JumpHeld) jumpBonusStorage = jumpBonus;
			else jumpBonusStorage = nonJumpMultiplier;

			if (other.collider.TryGetComponent(out IPlayerController controller)) controller.ApplyVelocity(Vector2.up * jumpBonusStorage * _bounceForce, PlayerForce.Burst);
		}
	}
}