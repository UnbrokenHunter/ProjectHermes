using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectHermes
{
    public class LevelCompletionBar : MonoBehaviour
    {

		#region Variables
		[HideIf("@slider != null")]
		[SerializeField] private Slider slider;

		[HideIf("@handle != null")]
		[SerializeField] private RectTransform handle;

		[SerializeField] private float checkpointLocation = 0;

		[Title("Colors")]
		[SerializeField] private Color unactivatedCheckpoint;
		[SerializeField] private Color activatedCheckpoint;

		// Calculate Player Position
		[SerializeField] private float PlayerPosition => transform.localScale.x - GameObject.Find("Player").transform.position.x;

		#endregion

		#region Methods

		private void Awake()
		{
			slider.maxValue = transform.localScale.x;
		}

		private void Update()
		{
			if (slider != null)
			{
				if (PlayerPosition <= transform.localScale.x)
				{
					slider.value = PlayerPosition;
				}

				// Find Checkpoint
				handle.transform.localPosition = new Vector3(checkpointLocation, 0, 0); 

				if(DoNotDestroy.instance.gameObject.GetComponentInChildren<CheckpointDoNotDestroy>().hasCheckpoint)
				{
					handle.gameObject.GetComponent<Image>().color = activatedCheckpoint;
				}
				else
				{
					handle.gameObject.GetComponent<Image>().color = unactivatedCheckpoint;
				}
				print(PlayerPosition);
			}
		}



		private void OnDrawGizmos()
		{
			Gizmos.color = Color.black;
			Gizmos.DrawIcon(transform.GetChild(0).position, transform.GetChild(0).lossyScale.ToString(), true);

			Gizmos.color = Color.red;
			Gizmos.DrawCube(transform.GetChild(0).position, transform.GetChild(0).lossyScale);
		}

		#endregion

	}
}
