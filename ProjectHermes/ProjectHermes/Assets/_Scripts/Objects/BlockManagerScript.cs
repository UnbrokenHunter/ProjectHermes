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
		private Rigidbody2D rb;

		private GameObject activePrefab => SetType();

		#endregion

		#region User Variables 

		[Title("Presets")]
		[OnValueChanged("SetType")]
		[EnumToggleButtons]
		[SerializeField] private BlockTypes blockType;

		[Space]

		[OnValueChanged("SetMoveType")]
		[EnumToggleButtons]
		[SerializeField] private MoveType moveType;

		[Space]

		[EnumToggleButtons]
		[SerializeField] private BlockVariant BlockVariants;

		#region Hideif Variables

		#region Blocks

		[HideIf("@BlockPrefab.Length >= 2")]
		[SerializeField] private GameObject[] BlockPrefab;

		#endregion

		#region Cloud

		[HideIf("@OneWayPrefabCloud != null")]
		[SerializeField] private GameObject OneWayPrefabCloud;

		[HideIf("@CloudSprite != null")]
		[SerializeField] private Sprite CloudSprite;

		[HideIf("@CloudEndLeft != null")]
		[SerializeField] private Sprite CloudEndLeft;

		[HideIf("@CloudEndRight != null")]
		[SerializeField] private Sprite CloudEndRight;

		[HideIf("@CloudEndBoth != null")]
		[SerializeField] private Sprite CloudEndBoth;

		#endregion

		#region Bridge

		[HideIf("@OneWayPrefabBridge != null")]
		[SerializeField] private GameObject OneWayPrefabBridge;

		#endregion

		#region Moving Platforms

		[HideIf("@patrolPlatform != null")]
		[SerializeField] private TarodevController.PatrolPlatform patrolPlatform;

		[HideIf("@radialPlatform != null")]
		[SerializeField] private TarodevController.RadialPlatform radialPlatform;

		#endregion

		#endregion

		[Space]

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

			#region BlockType

			if (blockType == BlockTypes.Block)
			{

				// Hitbox
				blockSpacing = 1.5f;

				yHitboxSize = 0.91f;
				xHitboxSize = 1f;
				yHitboxOffset = 0.04800171f;

				// Effects
				isOneWay = false;

				// Prefab
				if(BlockVariants == BlockVariant.Regular)
				{
					// Return Regular Variation
					return BlockPrefab[0];
				}
				else if (BlockVariants == BlockVariant.Colorful)
				{
					// Return Colorful Variation
					return BlockPrefab[1];
				}

				// To allow all paths to have a return value
				else
				{
					return null;
				}
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
				xHitboxSize = 1f;
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

			#endregion

		}

		private void SetMoveType()
		{
			#region MoveType

			rb = GetComponent<Rigidbody2D>();

			// If MoveType enum == static, set bodytype to static, otherwise set it to kinematic 
			rb.bodyType = moveType == MoveType.Static ? rb.bodyType = RigidbodyType2D.Static : RigidbodyType2D.Kinematic;

			if (moveType == MoveType.Static) { patrolPlatform.enabled = false; radialPlatform.enabled = false; }
			else if (moveType == MoveType.Patrol) { patrolPlatform.enabled = true; radialPlatform.enabled = false; }
			else if(moveType == MoveType.Radial) { patrolPlatform.enabled = false; radialPlatform.enabled = true; }

			#endregion
		}


		[HorizontalGroup("Split", 0.5f)]
		[TitleGroup("Split/Buttons")]
		[Button("Add Block", ButtonSizes.Large)]
		private void AddBlock()
		{
			// Handle Block Variants

			GameObject block = Instantiate(activePrefab, transform);
			blockList.Add(block);

			OrganizeBlocks();
		}

		[VerticalGroup("Split/Buttons/Right")]
		[Button("Remove Block", ButtonSizes.Large)]
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
	public enum MoveType
	{
		Static,
		Patrol,
		Radial
	}
	public enum BlockVariant
	{
		Regular, 
		Colorful

	}
}
