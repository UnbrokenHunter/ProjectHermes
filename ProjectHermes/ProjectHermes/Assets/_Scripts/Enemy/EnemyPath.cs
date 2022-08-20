using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class EnemyPath : MonoBehaviour
    {

        #region Variables

        [SerializeField] private float speed = 5;

        [ValueDropdown("Patrol_or_Coordinates")]
        [SerializeField] private bool isPatrol;

        
		[SerializeField] private Vector3[] pathPoint;
        [Space]

        [SerializeField] private Vector3 patrolRange = new Vector3(5, 0, 0);


        private Vector3 next;
        private SpriteRenderer _renderer;

        private Vector3 start;
        private Vector3 end;
        private bool goStart = false;

        #endregion


        #region Methods

        private void Awake()
        {
            next = pathPoint[0];

            _renderer = GetComponent<SpriteRenderer>();

            if(isPatrol)
			{
                start = transform.position;
                end = transform.position + patrolRange;
			}
        }

        private void Update()
        {
            moveToPath();
        }

        private void moveToPath()
		{
            if (!isPatrol)
            {
                for (int i = 0; i < pathPoint.Length; i++)
                {
                    if (transform.position == pathPoint[i])
                    {
                        if (i == pathPoint.Length - 1)
                        {
                            next = pathPoint[0];
                        }
                        else
                        {
                            next = pathPoint[i + 1];
                        }
                    }
                }
                
                // Flip Sprite
                if (transform.position != new Vector3(next.x, next.y, next.z)) _renderer.flipX = (transform.position.x - next.x) < 0;
          
                transform.position = Vector3.MoveTowards(transform.position, next, Time.deltaTime * speed);
            }

            else
			{
                if(transform.position == start)
				{
                    goStart = false;
				}
                else if (transform.position == end)
				{
                    goStart = true;
				}
                if (goStart)
				{
                    _renderer.flipX = false;
                    transform.position = Vector3.MoveTowards(transform.position, start, Time.deltaTime * speed);

                }
                else
				{
                    _renderer.flipX = true;
                    transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime * speed);
				}


			}

		}

#endregion


        #region Inspector Stuff
		
            private static IEnumerable Patrol_or_Coordinates = new ValueDropdownList<bool>()
            {
                { "Patrol", true },
                { "Coordinates", false },
            };

        #endregion

	}
}