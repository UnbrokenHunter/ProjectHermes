using UnityEngine;

namespace TarodevController {
    [CreateAssetMenu]
    public class ScriptableStats : ScriptableObject {
        [Header("MOVEMENT")] [Tooltip("The players capacity to gain speed")]
        public float Acceleration = 120;

        [Tooltip("The top horizontal movement speed")]
        public float MaxSpeed = 14;

        [Tooltip("The pace at which the player comes to a stop")]
        public float Deceleration = 60;

        [Tooltip("Movement loss after stopping input mid-air")]
        public float AirDecelerationPenalty = 0.5f;

        [Tooltip("A constant downward force applied while grounded. Helps on vertical moving platforms and slopes")] [Range(0, -10)]
        public float GroundingForce = -1.5f;

        [Tooltip("Allow speed creeping on a controller. Lightly tilt for slow speed.")]
        public bool AllowCreeping;

        [Header("CROUCHING")] [Tooltip("A dead-zone for controllers to prevent unintended crouching")]
        public float CrouchInputThreshold = -0.5f;

        [Tooltip("A speed multiplier while crouching")]
        public float CrouchSpeedPenalty = 0.5f;

        [Tooltip("The amount of frames it takes to hit the full crouch speed penalty. Higher values provide more crouch sliding")]
        public int CrouchSlowdownFrames = 50;
        
        [Tooltip("Detection height offset from the top of the standing collider. Smaller values risk collisions when standing up")]
        public float CrouchBufferCheck = 0.1f;

        [Header("JUMP")] [Tooltip("The immediate velocity applied when jumping")]
        public float JumpPower = 36;

        [Tooltip("Enable double jump")] public bool AllowDoubleJump;

        [Tooltip("Clamps the maximum fall speed")]
        public float MaxFallSpeed = 40;

        [Tooltip("The players capacity to gain fall speed")]
        public float FallSpeed = 110;

        [Tooltip("The gravity multiplier added when jump is released early")]
        public float JumpEndEarlyGravityModifier = 3;

        [Tooltip("The fixed frames before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
        public int CoyoteFrames = 7;

        [Tooltip("The amount of fixed frames we buffer a jump. This allows jump input before actually hitting the ground")]
        public int JumpBufferFrames = 7;

        [Header("DASH")] [Tooltip("Allows the player to dash")]
        public bool AllowDash;

        [Tooltip("The velocity of the dash")] public float DashVelocity = 30;

        [Tooltip("How many fixed frames the dash will last")]
        public int DashDurationFrames = 20;

        [Tooltip("The horizontal speed retained when dash has completed")]
        public float DashEndHorizontalMultiplier = 0.25f;

        [Header("COLLISIONS")] [Tooltip("The detection distance for grounding and roof detection")]
        public float GrounderDistance = 0.1f;

        [Header("ATTACK")] [Tooltip("The fixed frame cooldown of your players basic attack")]
        public int AttackFrameCooldown = 6;

        [Header("EXTERNAL")] [Tooltip("The rate at which external velocity decays")]
        public int ExternalVelocityDecay = 100;
    }
}