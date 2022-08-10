using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class StarController : MonoBehaviour
    {

    	#region Variables
        
        private float starEffectLength;
        public bool hasStarEffect = false;
        private SpriteRenderer _renderer;

		#endregion


		#region Methods

		private void Awake()
		{
            _renderer = GetComponentInChildren<SpriteRenderer>();
		}

		public void EnableStarEffect(float effectLength)
		{
            hasStarEffect = true;
            starEffectLength = effectLength;

            StartCoroutine(StarLength());

		}

        private IEnumerator StarLength()
		{
            yield return new WaitForSeconds(starEffectLength);
            hasStarEffect = false;
		}

        private void Update()
        {        
            if(hasStarEffect)
			{
                _renderer.color = new Color(Random.Range(1, 255), Random.Range(1, 255), Random.Range(1, 255));
			}
            else
			{
                _renderer.color = Color.white;
			}
        }

    	#endregion

    }
}
