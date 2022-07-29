using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectHermes
{
	public class PlayerMove : MonoBehaviour
    {

        #region Variables


        [FoldoutGroup("Variables")] public Slider MoveSlider;
        [FoldoutGroup("Variables")] public Slider JumpSlider;
        private Rigidbody2D rb;
        private CapsuleCollider2D col;

        // Collision

        // Movement
        [SerializeField] private float _currentVerticalSpeed;
        [SerializeField] private float _currentHorizontalSpeed;


        // Game Objects



        #endregion


        #region Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            Collisions();
            Move();
            Jump();
            ApplyGravity();

            ApplyMovement();
        }

        #region Collision 

        // Variables
        [FoldoutGroup("Variables")][SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private bool grounded;
        [SerializeField] private bool ceiling;
        [SerializeField] [FoldoutGroup("Variables")] private BoxCollider2D CeilingCollider;

		private void Collisions()
		{
            grounded = IsGrounded();

        }

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.isTrigger)
			{
                ceiling = true;
            }
		}

		#endregion

		#region Move

		private void Move()
		{
            //_currentHorizontalSpeed = MoveSlider.value * 5;
            _currentHorizontalSpeed = Input.GetAxisRaw("Horizontal") * 5;

        }

        #endregion

        #region Jump

        // Variables
        [HorizontalGroup("Split")]
        [BoxGroup("Split/Jump")]
        [SerializeField]
        [Range(0, 25)]
        [LabelWidth(110)]
        private float jumpHeight = 5;
		
        
        private void Jump()
        {
            if (grounded)
            {
                _currentVerticalSpeed = jumpHeight * JumpSlider.value;
                if (Input.GetButton("Jump")) _currentVerticalSpeed = jumpHeight;
 
            }           
            
            if(ceiling)
			{
                _currentVerticalSpeed = -_currentVerticalSpeed/2; 
			}
        }

        #endregion

        #region Gravity

        // Variables
        [HorizontalGroup("Split/Left")] 
        [BoxGroup("Split/Left/Gravity")] 
        [SerializeField] 
        [Range(0, 50)]
        [LabelWidth(110)]
        private float fallClamp = 20;

        [BoxGroup("Split/Left/Gravity")] 
        [SerializeField]
        [Range(0, 50)]
        [LabelWidth(110)]
        private float fallAcceleration = 5;

        private void ApplyGravity()
		{
            if (grounded)
            {
                // // Move out of the ground
                if (_currentVerticalSpeed < 0)
                {
                    _currentVerticalSpeed = 0;
                }
            }
            else
			{
                _currentVerticalSpeed -= fallAcceleration * Time.deltaTime;
                if(_currentVerticalSpeed <= -fallClamp)
				{
                    _currentVerticalSpeed = -fallClamp;
				}
			}
        }

		#endregion

		private void ApplyMovement()
		{
            rb.velocity = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed);
		}

		private bool IsGrounded()
        {
            float extraHeightText = .1f;
            RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, extraHeightText, groundLayerMask);
            Color rayColor;
            if (hit.collider != null)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }

            Debug.DrawRay(col.bounds.center + new Vector3(col.bounds.extents.x, 0), Vector2.down * (col.bounds.extents.y + extraHeightText), rayColor);
            Debug.DrawRay(col.bounds.center - new Vector3(col.bounds.extents.x, 0), Vector2.down * (col.bounds.extents.y + extraHeightText), rayColor);
            Debug.DrawRay(col.bounds.center - new Vector3(col.bounds.extents.x, col.bounds.extents.y + extraHeightText), Vector2.right * (col.bounds.extents.x * 2), rayColor);

            // Debug.Log(hit.collider);
            return hit.collider != null;

        }

        #endregion

    }
}
