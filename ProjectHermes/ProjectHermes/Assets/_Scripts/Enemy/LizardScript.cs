using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class LizardScript : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D[] boxs;
		private SpriteRenderer _ren;

		[SerializeField] [Range(0, 1)] private float _spriteOpacity;
		[SerializeField] private float invisablityLength;
        [SerializeField] private float timer = 0f;
		private bool invisable = false;
		private int defaultLayer;
		[SerializeField] private int invisableLayer;

		private void Awake()
		{
			_ren = GetComponent<SpriteRenderer>();
			defaultLayer = gameObject.layer;
		}

		private void Update()
		{
			timer += Time.deltaTime;

			if(timer >= invisablityLength)
			{
				invisable = !invisable ? true : false;
				timer = 0f;

				ChangeState();
			}
		}

		private void ChangeState()
		{
			foreach(var box in boxs)
			{
				box.enabled = !invisable;
				_ren.color = invisable ? new Color(1, 1, 1, _spriteOpacity) : Color.white;
			}
			gameObject.layer = !invisable ? defaultLayer : invisableLayer;
		}
	}
}
