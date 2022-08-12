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

        [SerializeField] private Material starMat;
        private Material previousMat;

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

            previousMat = _renderer.material;
            _renderer.material = starMat;

            StartCoroutine(StarLength());

		}

        private IEnumerator StarLength()
		{
            yield return new WaitForSeconds(starEffectLength);
            hasStarEffect = false;
            _renderer.material = previousMat;
		}

    	#endregion

    }
}
