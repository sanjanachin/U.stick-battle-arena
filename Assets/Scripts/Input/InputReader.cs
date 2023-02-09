using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Game
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IPlayerActions//, GameInput.IMenusActions
    {
        // Player
        public event UnityAction<Vector2> moveEvent = delegate { };
        public event UnityAction useItemDownEvent = delegate { };
        public event UnityAction useItemUpEvent = delegate { };
        public event UnityAction switchItemEvent = delegate { };

        // Menus
        // public event UnityAction<Vector2> menuMoveSelectionEvent = delegate { };
        // public event UnityAction menuConfirmEvent = delegate { };
        // public event UnityAction menuCancelEvent = delegate { };

        // !!! Remember to edit Input Reader functions upon updating the input map !!!
        private GameInput _gameInput;

        private void OnEnable() {
            if (_gameInput == null) {
                _gameInput = new GameInput();

                _gameInput.Player.SetCallbacks(this);
                // gameInput.Menus.SetCallbacks(this);
            }

            EnablePlayerInput();
        }

        private void OnDisable() {

        }

        // -----PLAYER-----
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

        // -----MENUS-----
        // public void OnMenuMoveSelection(InputAction.CallbackContext context)
        // {
        //     menuMoveSelectionEvent.Invoke(context.ReadValue<Vector2>());
        // }

        // public void OnMenuConfirm(InputAction.CallbackContext context)
        // {
        //     if (context.phase == InputActionPhase.Performed)
        //         menuConfirmEvent.Invoke();
        // }

        // public void OnMenuCancel(InputAction.CallbackContext context)
        // {
        //     if (context.phase == InputActionPhase.Performed)
        //         menuCancelEvent.Invoke();
        // }

        // Input Reader
        public void EnablePlayerInput() {
            _gameInput.Player.Enable();
        }

        // public void EnableMenusInput() {
        //     gameInput.Pointer.Disable();

        //     gameInput.Menus.Enable();
        // }

        public void DisableAllInput() {
            _gameInput.Player.Disable();
        }
    }
}