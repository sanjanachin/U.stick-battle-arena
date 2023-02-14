using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Game.Player {
    public class PlayerAnimator : MonoBehaviour {
        // [SerializeField] private float _minImpactForce = 20;
        
        // Anim times can be gathered from the state itself, but 
        // for the simplicity of the video...
        // [SerializeField] private float _landAnimDuration = 0.1f;
        // [SerializeField] private float _attackAnimTime = 0.2f;

        // maxTilt and tiltSpeed control rate and maximum pitch of rotation effect while moving
        [SerializeField] private float _maxTilt = 4;
        [SerializeField] private float _tiltSpeed = 20;

        private IPlayerController _player;
        private SpriteRenderer _renderer;
        
        private Animator _anim;
        private float _lockedTill;



        private void Awake() {
            if (!TryGetComponent(out IPlayerController player)) {
                Destroy(this);
                return;
            }

            _player = player;
            _anim = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            // if (_player.Input.x != 0) _renderer.flipX = _player.Input.x < 0;
            if (_player.MovementInput.X != 0) transform.localScale = new Vector3(_player.MovementInput.X < 0 ? 1 : -1, 1, 1);
            // Apply rotation to model dependant of speed and time
            var targetRotVector = new Vector3(0, 0, Mathf.Lerp(-_maxTilt, _maxTilt, Mathf.InverseLerp(-1, 1, _player.MovementInput.X)));
            _anim.transform.rotation = Quaternion.RotateTowards(_anim.transform.rotation, Quaternion.Euler(targetRotVector), _tiltSpeed * Time.deltaTime);
            var state = GetState();
            // Debug.Log(state);

            // _jumpTriggered = false;
            // _landed = false;
            // _attacked = false;

            if (state == _currentState) return;
            _anim.CrossFade(state, 0, 0);
            _currentState = state;
        }

        private int GetState() {
            // Catch the lock
            if (Time.time < _lockedTill) return _currentState;

            // Animations sorted by priority
            // if (_player.JumpingThisFrame) return Jump;
            if (_player.Grounded) {
                if (_player.MovementInput.X == 0) {
                    return Idle;
                } else {
                    return Walk;
                }
            } else {
                return Jump;
            }
            // if (_player.Grounded) return _player.MovementInput.X == 0 ? Idle : Walk;
            int LockState(int s, float t) {
                _lockedTill = Time.time + t;
                return s;
            }
            return Idle;

            // Priorities
            // if (_attacked) return LockState(Attack, _attackAnimTime);
            // if (_player.Crouching) return Crouch;
            // if (_landed) return LockState(Land, _landAnimDuration);

            // return _player.Speed.y > 0 ? Jump : Fall;

        }

        #region Cached Properties

        private int _currentState;

        private static readonly int Idle = Animator.StringToHash("IdleSpeed");
        private static readonly int Walk = Animator.StringToHash("Grounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        // private static readonly int Fall = Animator.StringToHash("Fall");
        // private static readonly int Land = Animator.StringToHash("Land");
        // private static readonly int Attack = Animator.StringToHash("Attack");
        // private static readonly int Crouch = Animator.StringToHash("Crouch");

        #endregion
    }

    // public interface IPlayerController {
    //     public Vector2 Input { get; }
    //     public Vector2 Speed { get; }
    //     public bool Crouching { get; }

    //     public event Action<bool, float> GroundedChanged; // Grounded - Impact force
    //     public event Action Jumped;
    //     public event Action Attacked;
    // }
}