using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Game
{
    [CreateAssetMenu(fileName = "PlayerActionInput", menuName = "Game/PlayerActionInput")]
    public class PlayerInputReader : ScriptableObject, PlayerInput.IPlayer1Actions, PlayerInput.IPlayer2Actions
    {
        // Player
        public event UnityAction<Vector2> moveEvent = delegate { };
        public event UnityAction useItemDownEvent = delegate { };
        public event UnityAction useItemUpEvent = delegate { };
        public event UnityAction switchItemEvent = delegate { };

        // !!! Remember to edit Input Reader functions upon updating the input map !!!
        private PlayerInput _gameInput;
        [SerializeField] private PlayerID _playerID;

        private void OnEnable() {
            if (_gameInput == null) {
                _gameInput = new PlayerInput();
                _gameInput.Player1.SetCallbacks(this);
                _gameInput.Player2.SetCallbacks(this);
            }

            DisableAllInput();
            
            // enable corresponding input for player id
            switch (_playerID)
            {
                case PlayerID.Player1:
                    EnablePlayer1Input();
                    break;
                case PlayerID.Player2:
                    EnablePlayer2Input();
                    break;
                default:
                    Debug.LogError($"Invalid player id {_playerID}");
                    DisableAllInput();
                    break;
            }
        }

        #region PlayerInput

        public void OnMovement(InputAction.CallbackContext context)
        {
            moveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                useItemDownEvent.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                useItemUpEvent.Invoke();
            }
        }

        public void OnSwitchItem(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                switchItemEvent.Invoke();
        }

        #endregion

        // Input Reader Controls
        public void EnablePlayer1Input() {
            DisableAllInput();
            _gameInput.Player1.Enable();
        }
        
        public void EnablePlayer2Input() {
            DisableAllInput();
            _gameInput.Player2.Enable();
        }

        public void DisableAllInput() {
            _gameInput.Player1.Disable();
            _gameInput.Player2.Disable();
        }
    }
}