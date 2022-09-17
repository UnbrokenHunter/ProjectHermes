using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

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

		[HideIf("@OneWayPrefabCloud != null")]
		[SerializeField] private GameObject OneWayPrefabCloud;

		[HideIf("@OneWayPrefabBridge != null")]
		[SerializeField] private GameObject OneWayPrefabBridge;

		[HideIf("@CloudSprite != null")]
		[SerializeField] private Sprite CloudSprite;

		[HideIf("@CloudEndLeft != null")]
		[SerializeField] private Sprite CloudEndLeft;

		[HideIf("@CloudEndRight != null")]
		[SerializeField] private Sprite CloudEndRight;

		[HideIf("@CloudEndBoth != null")]
		[SerializeField] private Sprite CloudEndBoth;

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
		[SerializeField] private float xHitboxSize = 0.935f;

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
				xHitboxSize = 0f;
				yHitboxOffset = 0.04800171f;

				// Effects
				isOneWay = false;

				// Prefab
				return BlockPrefab;
			}
			else if (blockType == BlockTypes.Cloud)
			{

				// Hitbox
				blockSpacing = 2.99f;

				yHitboxSize = 0.7998257f;
				xHitboxSize = 2.99f;
				yHitboxOffset = 0.08578894f;

				// Effects
				isOneWay = true;

				// Prefab
				return OneWayPrefabCloud;
			}
			else if (blockType == BlockTypes.OneWayPlatform)
			{

				// Hitbox
				blockSpacing = 1f;

				yHitboxSize = 0.4f;
				xHitboxSize = 0f;
				yHitboxOffset = .14f;

				// Effects
				isOneWay = true;

				// Prefab
				return OneWayPrefabBridge;
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

			FixCloudSprite();

			FixHitbox();
		}

		private void FixCloudSprite()
		{
			if(blockType == BlockTypes.Cloud)
			{
				if(blockList.Count > 1)
				{
					foreach(var block in blockList)
					{
						block.transform.GetComponent<SpriteRenderer>().sprite = CloudSprite;
					}

					// Left Side
					blockList[0].transform.GetComponentInChildren<SpriteRenderer>().sprite = CloudEndLeft;

					// Right Side
					blockList[blockList.Count - 1].transform.GetComponentInChildren<SpriteRenderer>().sprite = CloudEndRight;
				}

				if(blockList.Count == 1)
				{
					// One Platform
					blockList[0].transform.GetComponentInChildren<SpriteRenderer>().sprite = CloudEndBoth;
				}
			}
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
				if (blockType != BlockTypes.Block)
				{
					// If a child has a hitbox, turn it off, otherwise turn off parent hitbox
					if (blockList[i].transform.TryGetComponent<BoxCollider2D>(out BoxCollider2D boxs))
					{
						boxs.usedByComposite = true;

						boxs.size = new Vector2(xHitboxSize, yHitboxSize);
						boxs.offset = new Vector2(0, yHitboxOffset);
					}
				}
				else
				{
					if (blockList[i].transform.GetChild(0).TryGetComponent<BoxCollider2D>(out BoxCollider2D boxChild))
					{
						boxChild.usedByComposite = true;


						boxChild.size = new Vector2(xHitboxSize, yHitboxSize);
						boxChild.offset = new Vector2(0, yHitboxOffset);
					}
				}
			}
		}

		#endregion

	}

	public enum BlockTypes
	{
		Block,
		Cloud,
		OneWayPlatform

	}
}
