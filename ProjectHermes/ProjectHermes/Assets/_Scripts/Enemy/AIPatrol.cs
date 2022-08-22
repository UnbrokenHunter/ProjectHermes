using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class AIPatrol : MonoBehaviour
    {
		#region Variables

		#region Program Variables

		[HideInInspector]
        public bool mustPatrol;
		private bool mustFlip;

        private Rigidbody2D _rb;
		private float flipTimer;

		#endregion

		#region User Variables
		[Header("Patrol Type")]
		[ValueDropdown("Patrol_or_Coordinates")]
		[HideLabel]
		[SerializeField] private bool isPatrol;

		[HideIfGroup("isPatrol")]
		[SerializeField] private float walkDistance = 5;

		[Header("Walk Variables")]
		[SerializeField] private float _walkSpeed = 50;
		[SerializeField] private bool _startLeft = false;

		[Header("Physics Variables")]
		[SerializeField] private Transform groundCheckPos;
		[SerializeField] private Transform wallCheckPos;
		[SerializeField] [Range(0, .5f)] private float _checkRadius = .01f;
		[SerializeField] private LayerMask _layerMask;
		

		#endregion

		#endregion

		#region Methods

		#region Unity Methods

		private void Start()
		{
			mustPatrol = true;
			_rb = GetComponent<Rigidbody2D>();

			if(_startLeft) Flip();
		}

		private void FixedUpdate()
		{
			if(mustPatrol)
			{
				Patrol();
				if (isPatrol)
				{
					mustFlip = !Physics2D.OverlapCircle(groundCheckPos.position, _checkRadius, _layerMask) 
						|| Physics2D.OverlapCircle(wallCheckPos.position, _checkRadius, _layerMask);
				}
				if(!isPatrol)
				{
					flipTimer += Time.deltaTime;
					if(flipTimer >= walkDistance)
					{
						Flip();
					}
				}
			}
		}

		#endregion

		#region My Methods

		private void Patrol()
		{
			if (mustFlip) Flip();

			_rb.velocity = new Vector2(_walkSpeed * Time.deltaTime, _rb.velocity.y);
		}

		private void Flip()
		{
			mustPatrol = false;
			flipTimer = 0;
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
			_walkSpeed *= -1;
			mustPatrol = true;
		}

		#endregion

		#endregion

		#region Inspector Stuff

		private static IEnumerable Patrol_or_Coordinates = new ValueDropdownList<bool>()
			{
				{ "Patrol", true },
				{ "Distance", false },
			};

		#endregion
	}
}
