using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
	[ExecuteAlways]
	public class BlockManagerScript : MonoBehaviour
	{
		#region Program Variables

		private CompositeCollider2D box;

		private GameObject activePrefab => SetType();

		#endregion

		#region User Variables 

		[Title("Presets")]
		[OnValueChanged("SetType")]
		[EnumPaging]
		[SerializeField] private BlockTypes blockType;




		[HideIf("@BlockPrefab != null")]
		[SerializeField] private GameObject BlockPrefab;

		[HideIf("@OneWayPrefab != null")]
		[SerializeField] private GameObject OneWayPrefab;

		[ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false)]
		[SerializeField] private List<GameObject> blockList;


		[Title("Effects")]
		[Tooltip("Should the object being spawned be a have a oneway platform effector?")]
		[OnValueChanged("OrganizeBlocks")]
		[SerializeField] private bool isOneWay = false;


		// Hitbox 
		[Title("Hitbox Settings")]
		[OnValueChanged("OrganizeBlocks")]
		[SerializeField] private float blockSpacing = 1.5f;

		[OnValueChanged("OrganizeBlocks")]
		[SerializeField] private float yHitboxSize = 0.935f;

		[OnValueChanged("OrganizeBlocks")]
		[SerializeField] private float yHitboxOffset = 0.03200114f;

		#endregion

		#region Unity Methods

		#endregion

		#region User Methods


		private GameObject SetType()
		{
			if (blockType == BlockTypes.Block)
			{

				// Hitbox
				blockSpacing = 1.5f;

				yHitboxSize = 1.4025f;
				yHitboxOffset = 0.04800171f;

				// Effects
				isOneWay = false;

				// Prefab
				return BlockPrefab;
			}
			else if (blockType == BlockTypes.OneWayPlatform)
			{

				// Hitbox
				blockSpacing = 3.74f;

				yHitboxSize = 0.96f;
				yHitboxOffset = 0f;

				// Effects
				isOneWay = true;

				// Prefab
				return OneWayPrefab;
			}

			// To allow all paths to have a return value
			else
			{
				return null;
			}
		}

		[Button("Add Block")]
		private void AddBlock()
		{
			GameObject block = Instantiate(activePrefab, transform);
			blockList.Add(block);

			OrganizeBlocks();
		}

		[Button("Remove Block")]
		private void RemoveBlock()
		{
			if (blockList[blockList.Count - 1] != null)
			{
				DestroyImmediate(blockList[blockList.Count - 1]);

				blockList.RemoveAt(blockList.Count - 1);

			}

			OrganizeBlocks();
		}

		[Button("Update Order")]
		private void OrganizeBlocks()
		{
			for (int i = 0; i < blockList.Count; i++)
			{
				blockList[i].transform.position = transform.position + new Vector3(blockSpacing * i, 0, 0);
			}

			FixHitbox();
		}


		[Button("Update Hitbox")]
		private void FixHitbox()
		{
			box = GetComponent<CompositeCollider2D>();

			// One way
			box.usedByEffector = isOneWay;
			GetComponent<PlatformEffector2D>().enabled = isOneWay;


			for (int i = 0; i < blockList.Count; i++)
			{
				// If a child has a hitbox, turn it off, otherwise turn off parent hitbox
				if (blockList[i].transform.GetChild(0).TryGetComponent<BoxCollider2D>(out BoxCollider2D boxChild)) boxChild.usedByComposite = true;
				else if (blockList[i].transform.TryGetComponent<BoxCollider2D>(out BoxCollider2D boxs)) boxs.usedByComposite = true;
			}
		}

		#endregion

	}

	public enum BlockTypes
	{
		Block,
		OneWayPlatform,

	}
}
