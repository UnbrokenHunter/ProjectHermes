using UnityEngine;
using Sirenix.OdinInspector;

namespace TarodevController {
    [CreateAssetMenu]
    public class ScriptableStats : ScriptableObject {

		#region Movement

		[BoxGroup("Stats")]
        [HorizontalGroup("Stats/Split", 0.5f)]
        [VerticalGroup("Stats/Split/Left")]
        [BoxGroup("Stats/Split/Left/MOVEMENT")]
        [LabelWidth(250)]
        [Tooltip("The players capacity to gain speed")]
        public float Acceleration = 120;

        [BoxGroup("Stats/Split/Left/MOVEMENT")]
        [Tooltip("The top horizontal movement speed")]
        [LabelWidth(250)]
        public float MaxSpeed = 14;

        [BoxGroup("Stats/Split/Left/MOVEMENT")]
        [Tooltip("The pace at which the player comes to a stop")]
        [LabelWidth(250)]
        public float Deceleration = 60;

        [BoxGroup("Stats/Split/Left/MOVEMENT")]
        [Tooltip("Movement loss after stopping input mid-air")]
        [LabelWidth(250)]
        public float AirDecelerationPenalty = 0.5f;

        [BoxGroup("Stats/Split/Left/MOVEMENT")]
        [Tooltip("A constant downward force applied while grounded. Helps on vertical moving platforms and slopes")] [Range(0, -10)]
        [LabelWidth(250)]
        public float GroundingForce = -1.5f;

        [BoxGroup("Stats/Split/Left/MOVEMENT")]
        [Tooltip("Allow speed creeping on a controller. Lightly tilt for slow speed.")]
        [LabelWidth(250)]
        public bool AllowCreeping;

        #endregion

        #region Run

        [VerticalGroup("Stats/Split/Right")]

        [BoxGroup("Stats/Split/Right/RUN")]
        [LabelWidth(250)]
        public bool AllowRun = true;

        [BoxGroup("Stats/Split/Right/RUN")]
        [LabelWidth(250)]
        public float MaxRunSpeed = 15;

        #endregion

        #region Crouch

        [BoxGroup("Stats/Split/Left/CROUCH")]
        [Tooltip("A dead-zone for controllers to prevent unintended CROUCH")]
        [LabelWidth(250)]
        public float CrouchInputThreshold = -0.5f;

        [BoxGroup("Stats/Split/Left/CROUCH")]
        [Tooltip("A speed multiplier while CROUCH")]
        [LabelWidth(250)]
        public float CrouchSpeedPenalty = 0.5f;

        [BoxGroup("Stats/Split/Left/CROUCH")]
        [Tooltip("The amount of frames it takes to hit the full crouch speed penalty. Higher values provide more crouch sliding")]
        [LabelWidth(250)]
        public int CrouchSlowdownFrames = 50;

        [BoxGroup("Stats/Split/Left/CROUCH")]
        [Tooltip("Detection height offset from the top of the standing collider. Smaller values risk collisions when standing up")]
        [LabelWidth(250)]
        public float CrouchBufferCheck = 0.1f;

		#endregion

		#region Jump

        [BoxGroup("Stats/Split/Right/JUMP")]
        [Tooltip("The immediate velocity applied when jumping")]
        [LabelWidth(250)]
        public float JumpPower = 36;

        [BoxGroup("Stats/Split/Right/JUMP")]
        [Tooltip("Enable double jump")]
        [LabelWidth(250)]
        public bool AllowDoubleJump;

        [BoxGroup("Stats/Split/Right/JUMP")]
        [Tooltip("Clamps the maximum fall speed")]
        [LabelWidth(250)]
        public float MaxFallSpeed = 40;

        [BoxGroup("Stats/Split/Right/JUMP")]
        [Tooltip("The players capacity to gain fall speed")]
        [LabelWidth(250)]
        public float FallSpeed = 110;

        [BoxGroup("Stats/Split/Right/JUMP")]
        [Tooltip("The gravity multiplier added when jump is released early")]
        [LabelWidth(250)]
        public float JumpEndEarlyGravityModifier = 3;

        [BoxGroup("Stats/Split/Right/JUMP")]
        [Tooltip("The fixed frames before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
        [LabelWidth(250)]
        public int CoyoteFrames = 7;

        [BoxGroup("Stats/Split/Right/JUMP")]
        [Tooltip("The amount of fixed frames we buffer a jump. This allows jump input before actually hitting the ground")]
        [LabelWidth(250)]
        public int JumpBufferFrames = 7;

		#endregion

		#region Dash

		[BoxGroup("Stats/Split/Right/DASH")]
        [Tooltip("Allows the player to dash")]
        [LabelWidth(250)]
        public bool AllowDash;

        [BoxGroup("Stats/Split/Right/DASH")]
        [Tooltip("The velocity of the dash")]
        [LabelWidth(250)]
        public float DashVelocity = 30;

        [BoxGroup("Stats/Split/Right/DASH")]
        [Tooltip("How many fixed frames the dash will last")]
        [LabelWidth(250)]
        public int DashDurationFrames = 20;

        [BoxGroup("Stats/Split/Right/DASH")]
        [Tooltip("The horizontal speed retained when dash has completed")]
        [LabelWidth(250)]
        public float DashEndHorizontalMultiplier = 0.25f;

		#endregion

		#region Collisions

		[BoxGroup("Stats/Split/Left/COLLISIONS")]
        [Tooltip("The detection distance for grounding and roof detection")]
        [LabelWidth(250)]
        public float GrounderDistance = 0.1f;

		#endregion

		#region Attack

		[BoxGroup("Stats/Split/Right/ATTACK")]
        [Tooltip("The fixed frame cooldown of your players basic attack")]
        [LabelWidth(250)]
        public int AttackFrameCooldown = 6;

		#endregion

		#region External

		[BoxGroup("Stats/Split/Left/EXTERNAL")]
        [Tooltip("The rate at which external velocity decays")]
        [LabelWidth(250)]
        public int ExternalVelocityDecay = 100;

		#endregion

	}
}