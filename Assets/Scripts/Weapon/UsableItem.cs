using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class UsableItem : Pickable
    {
        public event UnityAction OnUseButtonDown = delegate {  }; 
        public event UnityAction OnUseButtonUp = delegate {  };

        protected bool _pressing = false;
        protected PlayerController _player;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        [SerializeField] private int _durability;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            OnPicked += RegisterToPlayer;
        }

        private void DisablePhysics()
        {
            _collider.isTrigger = true;
            _rigidbody.isKinematic = true;
        }
        
        // hook the player's events and disable physics to be held 
        private void RegisterToPlayer(PlayerController player)
        {
            player.OnItemUseDown += HandleItemUseDown;
            player.OnItemUseUp += HandleItemUseUp;
            DisablePhysics();
            _player = player;
        }

        private void HandleItemUseDown()
        {
            _pressing = true;
            OnUseButtonDown.Invoke();
        }
        
        private void HandleItemUseUp()
        {
            _pressing = false;
            OnUseButtonUp.Invoke();
        }

        protected void IncreaseDurability(int value) => _durability += value;
        protected void ReduceDurability(int value) => _durability -= value;
    }
}