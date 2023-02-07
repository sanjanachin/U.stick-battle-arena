using System;
using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    /**
     * Represent a item that can be used by the player using the
     * "use" button
     * Rely on Pickable component to be pick up by a player and to be used
     */
    [RequireComponent(typeof(Pickable))]
    public class UsableItem : MonoBehaviour
    {
        /**
         * Invoked when the player holding this item pressed the use button
         */
        public event UnityAction OnUseButtonDown = delegate {  }; 
        /**
         * Invoked when the player holding this item released the use button
         */
        public event UnityAction OnUseButtonUp = delegate {  };
        /**
         * Invoked when the durability of this item reaches 0
         */
        public event UnityAction OnBreak = delegate {  };

        public PlayerController Player { get => _player; }
        
        private bool _pressing = false;
        private PlayerController _player;

        [SerializeField] private GameplayService _service;
        [SerializeField] private Pickable _pickable;
        [SerializeField] private UsableItemID _id;
        [SerializeField] private int _durability;
        private float _lifespan;
        private bool _picked;

        protected virtual void Awake()
        {
            _pickable = GetComponent<Pickable>();

            _pickable.OnPicked += RegisterToPlayer;

            _picked = false;
        }

        private void Update()
        {
            if (!isActiveAndEnabled || _picked) return;
            
            if (_lifespan > 0)
            {
                _lifespan -= Time.deltaTime;
            }
            else
            {
                ReturnToPool();
            }
        }

        private void DisablePhysics()
        {
            _pickable.Collider.enabled = false;
            _pickable.Rigidbody.isKinematic = true;
        }
        
        // hook the player's events and disable physics to be held 
        private void RegisterToPlayer(PlayerController player)
        {
            player.OnItemUseDown += HandleItemUseDown;
            player.OnItemUseUp += HandleItemUseUp;
            DisablePhysics();
            _player = player;
            player.GetComponent<PlayerInventory>().PickUpItem(this);
            _picked = true;
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

        private void ReturnToPool()
        {
            _service.UsableItemManager.ReturnUsableItem(_id, this);
        }
        
        public void IncreaseDurability(int value) => _durability += value;

        public void ReduceDurability(int value)
        {
            _durability -= value;
            if (_durability <= 0)
                OnBreak.Invoke();
        }

        public void Spawn(float lifespan)
        {
            _lifespan = lifespan;
        }
    }
}