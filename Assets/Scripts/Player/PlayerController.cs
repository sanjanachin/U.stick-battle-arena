using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Game.Player
{
    // Script by Tarodev repo: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        // Public for external hooks
        public Vector3 Velocity { get; private set; }
        public FrameInput MovementInput { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector3 RawMovement { get; private set; }
        public bool FacingLeft => _facingLeft;
        public bool Grounded => _colDown;
        public PlayerStat Stat;

        [SerializeField] private PlayerInputReader _input;
        private bool _facingLeft = true;
        private Vector3 _lastPosition;
        private float _currentHorizontalSpeed, _currentVerticalSpeed;

        // This is horrible, but for some reason colliders are not fully established when update starts...
        private bool _active;

        public event UnityAction OnItemUseDown = () => { };
        public event UnityAction OnItemUseUp = () => { };
        public event UnityAction<Vector2> OnMovement = (_) => { };
        public event UnityAction OnSwitchItem = () => { };

        private void Awake()
        {
            // TODO: quick patch on input, should related to addressable, need to fix later
            _input = new PlayerInputReader();
            _input.moveEvent += UpdateInput;
            _input.useItemDownEvent += UseItemDown;
            _input.useItemUpEvent += UseItemUp;
            _input.switchItemEvent += SwitchItem;
            _input.EnablePlayerInput(GetComponent<PlayerStat>().ID);

            // mute control for 0.5 seconds
            Invoke(nameof(Activate), 0.5f);
        } 

        private void Activate() => _active = true;

        private void Update()
        {
            if (!_active) return;
            // Calculate velocity
            Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            RunCollisionChecks();

            CalculateWalk(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Actually perform the axis movement
        }
        
        private void UpdateInput(Vector2 value)
        {
            MovementInput = new FrameInput
            {
                JumpDown = value.y > 0,
                JumpUp = value.y < 0,
                X = value.x
            };
            
            if (MovementInput.JumpDown)
                _lastJumpPressed = Time.time;
            
            // update facing left flag
            if (MovementInput.X != 0)
                _facingLeft = MovementInput.X < 0;
            OnMovement.Invoke(value);
        }

        private void UseItemDown() => OnItemUseDown.Invoke();
        private void UseItemUp() => OnItemUseUp.Invoke();
        private void SwitchItem() => OnSwitchItem.Invoke();

        #region Collisions

        [Header("COLLISION")] [SerializeField] private Bounds _characterBounds;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;

        [SerializeField] [Range(0.1f, 0.3f)]
        private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

        private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        private bool _colUp, _colRight, _colDown, _colLeft;

        private float _timeLeftGrounded;

        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks()
        {
            // Generate ray ranges. 
            CalculateRayRanged();

            // Ground
            LandingThisFrame = false;
            var groundedCheck = RunDetection(_raysDown);
            if (_colDown && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
            else if (!_colDown && groundedCheck)
            {
                _coyoteUsable = true; // Only trigger when first touching
                LandingThisFrame = true;
            }

            _colDown = groundedCheck;

            // The rest
            _colUp = RunDetection(_raysUp);
            _colLeft = RunDetection(_raysLeft);
            _colRight = RunDetection(_raysRight);

            bool RunDetection(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point =>
                    Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            }
        }

        private void CalculateRayRanged()
        {
            // This is crying out for some kind of refactor. 
            var b = new Bounds(transform.position + _characterBounds.center, _characterBounds.size);

            _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
            _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
            _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
            _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
        }


        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        {
            for (var i = 0; i < _detectorCount; i++)
            {
                var t = (float) i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        private void OnDrawGizmos()
        {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying)
            {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> {_raysUp, _raysRight, _raysDown, _raysLeft})
                {
                    foreach (var point in EvaluateRayPositions(range))
                    {
                        Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            Gizmos.color = Color.red;
            var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center + move, _characterBounds.size);
        }

        #endregion

        #region Walk

        [Header("WALKING")] [SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _apexBonus = 2;

        private void CalculateWalk()
        {
            if (MovementInput.X != 0)
            {
                // Set horizontal move speed
                _currentHorizontalSpeed += MovementInput.X * _acceleration * Time.deltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(MovementInput.X) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else
            {
                // No input. Let's slow the character down
                _currentHorizontalSpeed =
                    Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
            }

            if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
            {
                // Don't walk through walls
                _currentHorizontalSpeed = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")] [SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity()
        {
            if (_colDown)
            {
                // Move out of the ground
                if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
            }
            else
            {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0
                    ? _fallSpeed * _jumpEndEarlyGravityModifier
                    : _fallSpeed;

                // Fall
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion

        #region Jump

        [Header("JUMPING")] [SerializeField] private float _jumpHeight = 30;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        private bool _coyoteUsable;
        private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed;
        private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

        private void CalculateJumpApex()
        {
            if (!_colDown)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
            {
                _apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (MovementInput.JumpDown && CanUseCoyote || HasBufferedJump)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
            }
            else
            {
                JumpingThisFrame = false;
            }

            // End the jump early if button released
            if (!_colDown && MovementInput.JumpUp && !_endedJumpEarly && Velocity.y > 0)
            {
                // _currentVerticalSpeed = 0;
                _endedJumpEarly = true;
            }

            if (_colUp)
            {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }
        }

        #endregion

        #region Move

        [Header("MOVE")]
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter()
        {
            var pos = transform.position + _characterBounds.center;
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            var move = RawMovement * Time.deltaTime;
            var furthestPoint = pos + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
            if (!hit)
            {
                transform.position += move;
                return;
            }

            // otherwise increment away from current pos; see what closest position we can move to
            var positionToMoveTo = transform.position;
            for (int i = 1; i < _freeColliderIterations; i++)
            {
                // increment to check all but furthestPoint - we did that already
                var t = (float) i / _freeColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer))
                {
                    transform.position = positionToMoveTo;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i == 1)
                    {
                        if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                        var dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }

                positionToMoveTo = posToTry;
            }
        }

        #endregion

        private void OnDestroy()
        {
            // TODO: ugly fix of double input in new game, need to rewrite
            _input.DisableAllInput();
            Object.Destroy(_input);
        }
    }

    public struct FrameInput
    {
        public float X;
        public bool JumpDown;
        public bool JumpUp;
    }

    public interface IPlayerController
    {
        public Vector3 Velocity { get; }
        public FrameInput MovementInput { get; }
        public bool JumpingThisFrame { get; }
        public bool LandingThisFrame { get; }
        public Vector3 RawMovement { get; }
        public bool Grounded { get; }
    }

    public struct RayRange
    {
        public RayRange(float x1, float y1, float x2, float y2, Vector2 dir)
        {
            Start = new Vector2(x1, y1);
            End = new Vector2(x2, y2);
            Dir = dir;
        }

        public readonly Vector2 Start, End, Dir;
    }
}