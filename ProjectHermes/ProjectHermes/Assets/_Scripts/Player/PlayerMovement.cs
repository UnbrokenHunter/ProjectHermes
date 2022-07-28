using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectHermes
{
	public class PlayerMovement : MonoBehaviour
	{
		// Movement
		[Header("Movement")]
		[SerializeField] private float _currentHorizontal;
		[SerializeField] private float _currentVertical;

		[Header("Speed")]
		[SerializeField] private float _speed;
		[SerializeField] private float _moveInput;
		[SerializeField] private float _maxSpeed;

		// Physics 
		[Header("Physics")]
		[SerializeField] private int _groundHitCount;
		private Rigidbody2D _rb;
		private CapsuleCollider2D _col; // Current collider
		private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[2];
		private readonly RaycastHit2D[] _ceilingHits = new RaycastHit2D[1];
		[SerializeField] private float GrounderDistance = 1.5f;
		[SerializeField] private bool _grounded;

		// Jump
		[Header("Jump")]
		[SerializeField] private float _jumpForce;
		[SerializeField] [Range(-4, 0)] private float _endJumpEarlyBonus;
		[SerializeField] private bool _jump;
		[SerializeField] private bool _jumpHeld;
		[SerializeField] private bool _isJumping;



		// Animation
		private SpriteRenderer _renderer;

		private void Start()
		{
			_rb = GetComponent<Rigidbody2D>();
			_col = GetComponent<CapsuleCollider2D>();
			_renderer = GetComponentInChildren<SpriteRenderer>();
		}

		private void FixedUpdate()
		{
			HandleCollision();
			HandleHorizontal();
			HandleJump();

			if (_moveInput != 0) _renderer.flipX = _moveInput < 0;
		}

		#region Collisions

		private void HandleCollision()
		{
			var offset = (Vector2)transform.position + _col.offset;

			_groundHitCount = Physics2D.CapsuleCastNonAlloc(offset, _col.size, _col.direction, 0, Vector2.down, _groundHits, GrounderDistance);
			var ceilingHits = Physics2D.CapsuleCastNonAlloc(offset, _col.size, _col.direction, 0, Vector2.up, _ceilingHits, GrounderDistance);

			if (ceilingHits > 0 && _currentHorizontal > 0) _currentHorizontal = 0;

			_grounded = _groundHitCount > 1 ? _grounded = true : _grounded = false;
		}

		#endregion

		#region Horizontal

		private void HandleHorizontal()
		{
			_moveInput = Input.GetAxisRaw("Horizontal");

			if(Mathf.Abs(_rb.velocity.x) > _maxSpeed)
			{
				_rb.velocity = new Vector2(_maxSpeed * _moveInput, _rb.velocity.y);
			}
			else
			{
				_rb.velocity = new Vector2(_speed * _moveInput, _rb.velocity.y);
			}
		}

		#endregion

		#region Jump

		private void HandleJump()
		{
			_jump = Input.GetButtonDown("Jump");
			_jumpHeld = Input.GetButton("Jump");

			// If press jump button and on the ground and are not already jumping
			if ((_jump || _jumpHeld) && _grounded && !_isJumping)
			{
				_rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
				_isJumping = true;
			}
			// If already jumping and not still holding the jump button
			else if (_isJumping && !_jumpHeld && !_grounded)
			{
				_rb.velocity += new Vector2(0, _endJumpEarlyBonus);
			}
			// If was jumping but not holding button and on the ground
			else if (_isJumping && !_jumpHeld && _grounded)
			{
				_isJumping = false;
			}
		}

		#endregion

	}
}
