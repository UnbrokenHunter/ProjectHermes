using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TarodevController;
using UnityEngine;

namespace ProjectHermes
{
    public class FireballController : MonoBehaviour
    {

		#region Variables
		private SpriteRenderer _renderer;

		public bool canShootFireball = true;

        public bool isFireUpgraded = false;
		public bool isInvincible = false;

		[Title("Player Settings")]

		[SerializeField] private Color deathFadeAmount;
		[SerializeField] private float invincibilityTime = 0.5f;

		[Title("Visuals")]

		[SerializeField] private GameObject fireballPrefab;
		[SerializeField] private Transform firePoint;
		[SerializeField] private Vector2 firePointPosition;
		[SerializeField] private Material regularMat;
		[SerializeField] private Material fireMat;
		[SerializeField] private GameObject Glow;

		[Title("Ability Settings")]

		[SerializeField] private float fireballSpeed;
		[SerializeField] private float fireballVerticalSpeed;
		[SerializeField] private float fireballBounce;
		[SerializeField] private float fireballLifetime;

		private bool lastFrame = false;

		#endregion


		#region Methods

		private void Awake()
		{
			_renderer = GetComponentInChildren<SpriteRenderer>();
		}

		private void Update()
		{

			firePoint.transform.localPosition = _renderer.flipX == true ? firePointPosition * -1 : firePointPosition;

			// If the state has changes and the previous state was not upgraded (not promoting)
			if (isFireUpgraded != lastFrame && lastFrame != false)
			{
				StartCoroutine(playerDemoted());
			}

			lastFrame = isFireUpgraded;

		}

		private IEnumerator playerDemoted()
		{
			isInvincible = true;

			AudioManager.instance.Play("PlayerDemoted");

			Color before = _renderer.color;

			// If renderer enabled, disable it, and vise versa
			_renderer.color = deathFadeAmount;

			yield return new WaitForSeconds(invincibilityTime);

			isInvincible = false;
			_renderer.material = regularMat;
			_renderer.color = before;
			Glow.SetActive(false);
		}

		public void shootFireball()
		{
			if (!canShootFireball) return;

			GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
			Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
			Transform transform = fireball.GetComponent<Transform>();
			SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();

			rb.gravityScale = fireballBounce;

			if(_renderer.flipX == true) transform.localRotation = new Quaternion(0, 180, 0, 0);
			Vector2 direction = _renderer.flipX == true ?  direction = Vector2.left : direction = Vector2.right;
			rb.AddForce(Vector2.down * fireballVerticalSpeed, ForceMode2D.Impulse);
			rb.AddForce(direction * fireballSpeed, ForceMode2D.Impulse);

			StartCoroutine(destroyFireball(fireball));

		}

		private IEnumerator destroyFireball(GameObject obj)
		{
			yield return new WaitForSeconds(fireballLifetime);
			
			Destroy(obj);
		}

		// Promote Player
		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.tag == "FireballUpgrade")
			{
                isFireUpgraded = true;
				_renderer.material = fireMat;
				Glow.SetActive(true);
				AudioManager.instance.Play("PowerUp");
				Destroy(other.gameObject);
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if(collision.gameObject.layer == 6)
			{
				Destroy(collision.gameObject);
			}
		}


		#endregion

	}
}
