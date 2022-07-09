using UnityEngine;
using UnityEngine.UI;
#if (ENABLE_INPUT_SYSTEM)
using UnityEngine.InputSystem;
#endif

namespace TarodevController {
    public class PlayerInput : MonoBehaviour {

        // Sliders
        [SerializeField] private Slider horizontalSlider;
        [SerializeField] private Slider verticalSlider;
        public int i = 0;
        public float inputBalance;

#if (ENABLE_LEGACY_INPUT_MANAGER)
        public FrameInput GatherInput() {

            if (i !< verticalSlider.maxValue)
			{
                i++;
			}


            return new FrameInput {
                JumpDown = verticalSlider.value > 0,

                JumpHeld = i < verticalSlider.value,
                DashDown = Input.GetButtonDown("Dash"),

                X = horizontalSlider.value,
                //Y = Input.GetAxisRaw("Vertical")
            };
        }
#elif (ENABLE_INPUT_SYSTEM)
        private PlayerInputActions _actions;
        private InputAction _move, _jump, _dash;

        private void Awake()
        {
            _actions = new PlayerInputActions();
            _move = _actions.Player.Move;
            _jump = _actions.Player.Jump;
            _dash = _actions.Player.Dash;
        }

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();

        public FrameInput GatherInput() {
            return new FrameInput {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                DashDown = _dash.WasPressedThisFrame(),

                X = _move.ReadValue<Vector2>().x,
                Y = _move.ReadValue<Vector2>().y
            };
        }
#endif
    }
}