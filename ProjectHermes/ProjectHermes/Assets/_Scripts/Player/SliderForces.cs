using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TarodevController;

namespace ProjectHermes
{
	public class SliderForces : MonoBehaviour
	{

		[SerializeField] private float force = 5;
		[SerializeField] private Slider slider;


		private void OnCollisionStay2D(Collision2D other)
		{
			if (other.gameObject.TryGetComponent(out IPlayerController controller))
			{
				controller.ApplyVelocity(new Vector2(force * slider.value, 0), PlayerForce.Burst);
			}
			else if (other.gameObject.tag == "Moveable Object")
			{
				Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
				rb.velocity = new Vector2(rb.velocity.x + force * slider.value, rb.velocity.y);
			}
		}

	}
}
