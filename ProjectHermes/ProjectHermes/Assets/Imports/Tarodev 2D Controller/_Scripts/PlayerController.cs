using System;
using System.Collections.Generic;
using UnityEngine;

namespace TarodevController {
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// Right now it only contains movement and jumping, but it should be pretty easy to expand... I may even do it myself
    /// if there's enough interest. You can play and compete for best times here: https://tarodev.itch.io/
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/GqeHHnhHpz
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IPlayerController {
        [SerializeField] private bool _allowDoubleJump, _allowDash, _allowCrouch;

        // Public for external hooks
        public FrameInput Input { get; private set; }
        public Vector2 RawMovement { get; private set; }
        public bool Grounded => _grounded;
        public event Action<bool> OnGroundedChanged;
        public event Action OnJumping, OnDoubleJumping;
        public event Action<bool> OnDashingChanged;
        public event Action<bool> OnCrouchingChanged;

        private Rigidbody2D _rb;
        private BoxCollider2D _collider;
        private PlayerInput _input;
        private Vector2 _lastPosition;
        private Vector2 _velocity;
        private Vector2 _speed;
        private int _fixedFrame;

        void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _input = GetComponent<PlayerInput>();

            _defaultColliderSize = _collider.size;
            _defaultColliderOffset = _collider.offset;
        }

        private void Update() => GatherInput();

        void FixedUpdate() {
            _fixedFrame++;
            _frameClamp = _moveClamp;

            // Calculate velocity
            _velocity = (_rb.position - _lastPosition) / Time.fixedDeltaTime;
            _lastPosition = _rb.position;

            RunCollisionChecks();

            CalculateCrouch();
            CalculateHorizontal();
            CalculateJumpApex();
            CalculateGravity();
            CalculateJump();
            CalculateDash();
            MoveCharacter();
        }

        #region Gather Input

        private void GatherInput() {
            Input = _input.GatherInput();

            if (Input.DashDown) _dashToConsume = true;
            if (Input.JumpDown) {
                _lastJumpPressed = _fixedFrame;
                _jumpToConsume = true;
            }
        }

        #endregion

        #region Collisions

        [Header("COLLISION")] [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _detectionRayLength = 0.1f;
        private RaycastHit2D[] _hitsDown = new RaycastHit2D[3];
        private RaycastHit2D[] _hitsUp = new RaycastHit2D[1];
        private RaycastHit2D[] _hitsLeft = new RaycastHit2D[1];
        private RaycastHit2D[] _hitsRight = new RaycastHit2D[1];

        private bool _hittingCeiling, _grounded, _colRight, _colLeft;

        private float _timeLeftGrounded;


        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks() {
            // Generate ray ranges. 
            var b = _collider.bounds;

            // Ground
            var groundedCheck = RunDetection(Vector2.down, out _hitsDown);
            _colLeft = RunDetection(Vector2.left, out _hitsLeft);
            _colRight = RunDetection(Vector2.right, out _hitsRight);
            _hittingCeiling = RunDetection(Vector2.up, out _hitsUp);

            if (_grounded && !groundedCheck) {
                _timeLeftGrounded = _fixedFrame; // Only trigger when first leaving
                OnGroundedChanged?.Invoke(false);
            }
            else if (!_grounded && groundedCheck) {
                _coyoteUsable = true; // Only trigger when first touching
                _executedBufferedJump = false;
                _doubleJumpUsable = true;
                _canDash = true;
                OnGroundedChanged?.Invoke(true);
                _speed.y = 0;
            }

            _grounded = groundedCheck;

            bool RunDetection(Vector2 dir, out RaycastHit2D[] hits) {
                // Array.Clear(hits, 0, hits.Length);
                // Physics2D.BoxCastNonAlloc(b.center, b.size, 0, dir, hits, _detectionRayLength, _groundLayer);

                // This produces garbage, but is significantly more performant. Also less buggy.
                hits = Physics2D.BoxCastAll(b.center, b.size, 0, dir, _detectionRayLength, _groundLayer);

                foreach (var hit in hits)
                    if (hit.collider && !hit.collider.isTrigger)
                        return true;
                return false;
            }
        }

        private void OnDrawGizmos() {
            if (!_collider) _collider = GetComponent<BoxCollider2D>();

            Gizmos.color = Color.blue;
            var b = _collider.bounds;
            b.Expand(_detectionRayLength);

            Gizmos.DrawWireCube(b.center, b.size);
        }

        #endregion

        #region Crouch

        [SerializeField, Header("CROUCH")] private float _crouchSizeModifier = 0.5f;
        [SerializeField] private float _crouchSpeedModifier = 0.1f;
        [SerializeField] private int _crouchSlowdownFrames = 50;
        [SerializeField] private float _immediateCrouchSlowdownThreshold = 0.1f;
        private Vector2 _defaultColliderSize, _defaultColliderOffset;
        private float _velocityOnCrouch;
        private bool _crouching;
        private int _frameStartedCrouching;

        private bool CanStand {
            get {
                var col = Physics2D.OverlapBox((Vector2)transform.position + _defaultColliderOffset, _defaultColliderSize * 0.95f, 0, _groundLayer);
                return (col == null || col.isTrigger);
            }
        }

        void CalculateCrouch() {
            if (!_allowCrouch) return;


            if (_crouching) {
                var immediate = _velocityOnCrouch <= _immediateCrouchSlowdownThreshold ? _crouchSlowdownFrames : 0;
                var crouchPoint = Mathf.InverseLerp(0, _crouchSlowdownFrames, _fixedFrame - _frameStartedCrouching + immediate);
                _frameClamp *= Mathf.Lerp(1, _crouchSpeedModifier, crouchPoint);
            }

            if (_grounded && Input.Y < 0 && !_crouching) {
                _crouching = true;
                OnCrouchingChanged?.Invoke(true);
                _velocityOnCrouch = Mathf.Abs(_velocity.x);
                _frameStartedCrouching = _fixedFrame;

                _collider.size = _defaultColliderSize * new Vector2(1, _crouchSizeModifier);

                // Lower the collider by the difference extent
                var difference = _defaultColliderSize.y - (_defaultColliderSize.y * _crouchSizeModifier);
                _collider.offset = -new Vector2(0, difference * 0.5f);
            }
            else if (!_grounded || (Input.Y >= 0 && _crouching)) {
                // Detect obstruction in standing area. Add a .1 y buffer to avoid the ground.
                if (!CanStand) return;

                _crouching = false;
                OnCrouchingChanged?.Invoke(false);

                _collider.size = _defaultColliderSize;
                _collider.offset = _defaultColliderOffset;
            }
        }

        #endregion

        #region Horizontal

        [Header("WALKING")] [SerializeField] private float _acceleration = 120;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deceleration = 60f;
        [SerializeField] private float _apexBonus = 100;

        [SerializeField] private bool _allowCreeping;

        private float _frameClamp;

        private void CalculateHorizontal() {
            if (Input.X != 0) {
                // Set horizontal move speed
                if (_allowCreeping) _speed.x = Mathf.MoveTowards(_speed.x, _frameClamp * Input.X, _acceleration * Time.fixedDeltaTime);
                else _speed.x += Input.X * _acceleration * Time.fixedDeltaTime;

                // Clamped by max frame movement
                _speed.x = Mathf.Clamp(_speed.x, -_frameClamp, _frameClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
                _speed.x += apexBonus * Time.fixedDeltaTime;
            }
            else {
                // No input. Let's slow the character down
                _speed.x = Mathf.MoveTowards(_speed.x, 0, _deceleration * Time.fixedDeltaTime);
            }

            if (!_grounded && (_speed.x > 0 && _colRight || _speed.x < 0 && _colLeft)) {
                // Don't pile up useless horizontal (prevents sticking to walls mid-air)
                _speed.x = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")] [SerializeField] private float _fallClamp = -60f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 160f;
        [SerializeField, Range(0, -10)] private float _groundingForce = -1.5f;
        private float _fallSpeed;
        
        private void CalculateGravity() {
            if (_grounded) {
                if (Input.X == 0)  return;

                // Slopes
                _speed.y = _groundingForce;
                foreach (var hit in _hitsDown) {
                    if (hit.collider.isTrigger) continue;
                    var slopePerp = Vector2.Perpendicular(hit.normal).normalized;

                    var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    // This needs improvement. Prioritize front hit for smoother slope apex
                    if (slopeAngle != 0) {
                        _speed.y = _speed.x * -slopePerp.y;
                        _speed.y += _groundingForce; // Honestly, this feels like cheating. I'll work on it
                        break;
                    }
                }
            }
            else {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _speed.y > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                // Fall
                _speed.y -= fallSpeed * Time.fixedDeltaTime;

                // Clamp
                if (_speed.y < _fallClamp) _speed.y = _fallClamp;
            }
        }

        #endregion

        #region Jump

        [Header("JUMPING")] [SerializeField] private float _jumpHeight = 35;
        [SerializeField] private float _jumpApexThreshold = 40f;
        [SerializeField] private int _coyoteTimeThreshold = 7;
        [SerializeField] private int _jumpBuffer = 7;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        private bool _jumpToConsume;
        private bool _coyoteUsable;
        private bool _executedBufferedJump;
        private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed = float.MinValue;
        private bool _doubleJumpUsable;

        private bool CanUseCoyote => _coyoteUsable && !_grounded && _timeLeftGrounded + _coyoteTimeThreshold > _fixedFrame;
        private bool HasBufferedJump => ((_grounded && !_executedBufferedJump) || _cornerStuck) && _lastJumpPressed + _jumpBuffer > _fixedFrame;
        private bool CanDoubleJump => _allowDoubleJump && _doubleJumpUsable && !_coyoteUsable;

        private void CalculateJumpApex() {
            if (!_grounded) {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else {
                _apexPoint = 0;
            }
        }

        private void CalculateJump() {
            if (_crouching && !CanStand) return;

            if (_jumpToConsume && CanDoubleJump) {
                _speed.y = _jumpHeight;
                _doubleJumpUsable = false;
                _endedJumpEarly = false;
                _jumpToConsume = false;
                OnDoubleJumping?.Invoke();
            }

            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if ((_jumpToConsume && CanUseCoyote) || HasBufferedJump) {
                _speed.y = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _jumpToConsume = false;
                _timeLeftGrounded = _fixedFrame;
                _executedBufferedJump = true;
                OnJumping?.Invoke();
            }

            // End the jump early if button released
            if (!_grounded && !Input.JumpHeld && !_endedJumpEarly && _velocity.y > 0) _endedJumpEarly = true;

            if (_hittingCeiling && _speed.y > 0) _speed.y = 0;
        }

        #endregion

        #region Dash

        [Header("DASH")] [SerializeField] private float _dashPower = 30;
        [SerializeField] private int _dashLength = 6;
        [SerializeField] private float _dashEndHorizontalMultiplier = 0.25f;
        private float _startedDashing;
        private bool _canDash;
        private Vector2 _dashVel;

        private bool _dashing;
        private bool _dashToConsume;

        void CalculateDash() {
            if (!_allowDash) return;
            if (_dashToConsume && _canDash && !_crouching) {
                var vel = new Vector2(Input.X, _grounded && Input.Y < 0 ? 0 : Input.Y).normalized;
                if (vel == Vector2.zero) {
                    _dashToConsume = false;
                    return;
                }
                _dashVel = vel * _dashPower;
                _dashing = true;
                OnDashingChanged?.Invoke(true);
                _canDash = false;
                _startedDashing = _fixedFrame;

                // Strip external buildup
                _forceBuildup = Vector2.zero;
            }

            if (_dashing) {
                _speed.x = _dashVel.x;
                _speed.y = _dashVel.y;
                // Cancel when the time is out or we've reached our max safety distance
                if (_startedDashing + _dashLength < _fixedFrame) {
                    _dashing = false;
                    OnDashingChanged?.Invoke(false);
                    if (_speed.y > 0) _speed.y = 0;
                    _speed.x *= _dashEndHorizontalMultiplier;
                    if (_grounded) _canDash = true;
                }
            }

            _dashToConsume = false;
        }

        #endregion
        
        #region Move

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter() {
            RawMovement = _speed; // Used externally
            var move = RawMovement * Time.fixedDeltaTime;

            // Apply effectors
            move += EvaluateEffectors();

            move += EvaluateForces();

            _rb.MovePosition(_rb.position + move);

            RunCornerPrevention();
        }

        #region Corner Stuck Prevention

        private Vector2 _lastPos;
        private bool _cornerStuck;

        // This is a little hacky, but it's very difficult to fix.
        // This will allow walking and jumping while right on the corner of a ledge.
        void RunCornerPrevention() {
            // There's a fiddly thing where the rays will not detect ground (right inline with the collider),
            // but the collider won't fit. So we detect if we're meant to be moving but not.
            // The downside to this is if you stand still on a corner and jump straight up, it won't trigger the land
            // when you touch down. Sucks... but not sure how to go about it at this stage
            _cornerStuck = !_grounded && _lastPos == _rb.position && _lastJumpPressed + 1 < _fixedFrame;
            _speed.y = _cornerStuck ? 0 : _speed.y;
            _lastPos = _rb.position;
        }

        #endregion

        #endregion

        #region Effectors & Forces

        private readonly List<IPlayerEffector> _usedEffectors = new List<IPlayerEffector>();

        /// <summary>
        /// For more passive force effects like moving platforms, underwater etc
        /// </summary>
        /// <returns></returns>
        private Vector2 EvaluateEffectors() {
            var effectorDirection = Vector2.zero;
            // Repeat this for other directions and possibly even area effectors. Wind zones, underwater etc
            effectorDirection += Process(_hitsDown);

            _usedEffectors.Clear();
            return effectorDirection;

            Vector2 Process(IEnumerable<RaycastHit2D> hits) {
                foreach (var hit2D in hits) {
                    if (!hit2D.transform) return Vector2.zero;
                    if (hit2D.transform.TryGetComponent(out IPlayerEffector effector)) {
                        if (_usedEffectors.Contains(effector)) continue;
                        _usedEffectors.Add(effector);
                        return effector.EvaluateEffector();
                    }
                }

                return Vector2.zero;
            }
        }

        [Header("EFFECTORS")] [SerializeField] private float _forceDecay = 1;
        private Vector2 _forceBuildup;

        public void AddForce(Vector2 force, PlayerForce mode = PlayerForce.Burst, bool cancelMovement = true) {
            if (cancelMovement) _speed = Vector2.zero;

            switch (mode) {
                case PlayerForce.Burst:
                    _speed += force;
                    break;
                case PlayerForce.Decay:
                    _forceBuildup += force * Time.fixedDeltaTime;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        private Vector2 EvaluateForces() {
            // Prevent bouncing. This *could* cause problems, but I'm yet to find any
            if (_colLeft || _colRight) _forceBuildup.x = 0;
            if (_grounded || _hittingCeiling) _forceBuildup.y = 0;

            var force = _forceBuildup;

            _forceBuildup = Vector2.MoveTowards(_forceBuildup, Vector2.zero, _forceDecay * Time.fixedDeltaTime);

            return force;
        }

        #endregion

        #region Misc

        private void OnValidate() {
            if(_groundLayer.value == 0) 
                Debug.LogError($"The controllers ground layer is set to <b>Nothing</b><br>. " +
                               $"Create a new layer called <b>Ground</b> (I use layer index 6), set walkable terrain to <b>Ground</b> " +
                               $"and assign it to the controllers GroundLayer.");
        }

        #endregion
    }
}