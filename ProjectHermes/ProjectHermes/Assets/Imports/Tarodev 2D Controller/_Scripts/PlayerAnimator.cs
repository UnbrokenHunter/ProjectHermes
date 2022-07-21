using UnityEngine;

namespace TarodevController {
    public class PlayerAnimator : MonoBehaviour {
        private IPlayerController _player;
        private Animator _anim;
        private SpriteRenderer _renderer;
        private AudioSource _source;

        private void Awake() {
            _player = GetComponentInParent<IPlayerController>();
            _anim = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _source = GetComponent<AudioSource>();
        }

        private void Start() {
            _player.Jumped += OnPlayerOnJumped;
            _player.DoubleJumped += PlayerOnDoubleJumped;
            _player.Attacked += OnPlayerOnAttacked;
            _player.GroundedChanged += OnPlayerOnGroundedChanged;
            _player.DashingChanged += PlayerOnDashingChanged;
        }

        private void Update() {
            if (_player.Input.x != 0) _renderer.flipX = _player.Input.x < 0;

            HandleGroundEffects();
            DetectGroundColor();
            HandleAnimations();
        }

        #region Ground movement

        [Header("GROUND MOVEMENT")] [SerializeField]
        private ParticleSystem _moveParticles;

        [SerializeField] private float _tileChangeSpeed = .05f;
        [SerializeField] private AudioClip[] _footstepClips;
        private ParticleSystem.MinMaxGradient _currentGradient;
        private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[2];
        private Vector2 _tiltVelocity;

        private void DetectGroundColor() {
            var hitCount = Physics2D.RaycastNonAlloc(transform.position, Vector3.down, _groundHits, 2);
            for (var i = 0; i < hitCount; i++) {
                var hit = _groundHits[i];
                if (!hit || hit.collider.isTrigger || !hit.transform.TryGetComponent(out SpriteRenderer r)) continue;
                var color = r.color;
                _currentGradient = new ParticleSystem.MinMaxGradient(color * 0.9f, color * 1.2f);
                SetColor(_moveParticles);
                return;
            }
        }

        private void SetColor(ParticleSystem ps) {
            var main = ps.main;
            main.startColor = _currentGradient;
        }

        private void HandleGroundEffects() {
            // Move particles get bigger as you gain momentum
            var speedPoint = Mathf.InverseLerp(0, _player.PlayerStats.MaxSpeed, Mathf.Abs(_player.Speed.x));
            _moveParticles.transform.localScale = Vector3.MoveTowards(_moveParticles.transform.localScale, Vector3.one * speedPoint, 2 * Time.deltaTime);

            // Tilt with slopes
            transform.up = Vector2.SmoothDamp(transform.up, _grounded ? _player.GroundNormal : Vector2.up, ref _tiltVelocity, _tileChangeSpeed);
        }

        private int _stepIndex;

        public void PlayFootstep() {
            PlaySound(_footstepClips[_stepIndex++ % _footstepClips.Length], 0.01f);
        }

        #endregion

        #region Jumping

        [Header("JUMPING")] [SerializeField] private float _minImpactForce = 20;
        [SerializeField] private float _landAnimDuration = 0.1f;
        [SerializeField] private AudioClip _landClip, _jumpClip, _doubleJumpClip;
        [SerializeField] private ParticleSystem _jumpParticles, _launchParticles, _doubleJumpParticles, _landParticles;

        private bool _jumpTriggered;
        private bool _landed;
        private bool _grounded;

        private void OnPlayerOnJumped() {
            _jumpTriggered = true;
            PlaySound(_jumpClip, 0.05f, Random.Range(0.98f, 1.02f));
            SetColor(_jumpParticles);
            SetColor(_launchParticles);
            _jumpParticles.Play();
        }

        private void PlayerOnDoubleJumped() {
            PlaySound(_doubleJumpClip, 0.1f);
            _doubleJumpParticles.Play();
        }

        private void OnPlayerOnGroundedChanged(bool grounded, float impactForce) {
            _grounded = grounded;
            var p = Mathf.InverseLerp(0, _minImpactForce, impactForce);

            if (impactForce >= _minImpactForce) {
                _landed = true;
                _landParticles.transform.localScale = p * Vector3.one;
                _landParticles.Play();
                SetColor(_landParticles);
                PlaySound(_landClip, p * 0.1f);
            }

            if (_grounded) _moveParticles.Play();
            else _moveParticles.Stop();
        }

        #endregion

        #region Dash

        [Header("DASHING")] [SerializeField] private AudioClip _dashClip;
        [SerializeField] private ParticleSystem _dashParticles, _dashRingParticles;
        [SerializeField] private Transform _dashRingTransform;

        private void PlayerOnDashingChanged(bool dashing, Vector2 dir) {
            if (dashing) {
                _dashRingTransform.up = dir;
                _dashRingParticles.Play();
                _dashParticles.Play();
                PlaySound(_dashClip, 0.1f);
            }
            else {
                _dashParticles.Stop();
            }
        }

        #endregion

        #region Attack

        [Header("ATTACK")] [SerializeField] private float _attackAnimTime = 0.2f;
        [SerializeField] private AudioClip _attackClip;
        private bool _attacked;

        private void OnPlayerOnAttacked() {
            _attacked = true;
            PlaySound(_attackClip, 0.1f, Random.Range(0.97f, 1.03f));
        }

        #endregion

        #region Animation

        private float _lockedTill;

        private void HandleAnimations() {
            var state = GetState();

            _jumpTriggered = false;
            _landed = false;
            _attacked = false;

            if (state == _currentState) return;
            _anim.CrossFade(state, 0, 0);
            _currentState = state;

            int GetState() {
                if (Time.time < _lockedTill) return _currentState;

                // Priorities
                if (_attacked) return LockState(Attack, _attackAnimTime);
                if (_player.Crouching) return Crouch;
                if (_landed) return LockState(Land, _landAnimDuration);
                if (_jumpTriggered) return Jump;

                if (_grounded) return _player.Input.x == 0 ? Idle : Walk;
                return _player.Speed.y > 0 ? Jump : Fall;

                int LockState(int s, float t) {
                    _lockedTill = Time.time + t;
                    return s;
                }
            }
        }

        #region Cached Properties

        private int _currentState;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Crouch = Animator.StringToHash("Crouch");

        #endregion

        #endregion

        private void PlaySound(AudioClip clip, float volume = 1, float pitch = 1) {
            _source.pitch = pitch;
            _source.PlayOneShot(clip, volume);
        }
    }
}