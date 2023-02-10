using Game.Player;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

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
        public event UnityAction<UsableItem> OnBreak = delegate {  };
        public event UnityAction OnReturn = delegate {  };

        public PlayerController Player { get => _player; }
        
        private PlayerController _player;

        [SerializeField] private GameplayService _service;
        [SerializeField] private Transform _transform;
        [SerializeField] private Pickable _pickable;
        [SerializeField] private UsableItemID _id;
        [SerializeField] private int _maxDurability;
        [SerializeField] private int _durability;
        private float _lifespan;
        private bool _picked => _player != null;
        private bool _equipped = false;

        protected virtual void Awake()
        {
            _transform = GetComponent<Transform>();
            _pickable = GetComponent<Pickable>();
            _durability = _maxDurability;

            _pickable.OnPicked += RegisterToPlayer;
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
            _pickable.Rigidbody.velocity = Vector2.zero;
            _pickable.Rigidbody.isKinematic = true;
        }
        
        private void EnablePhysics()
        {
            _pickable.Collider.enabled = true;
            _pickable.Rigidbody.isKinematic = false;
        }
        
        // hook the player's events and disable physics to be held 
        private void RegisterToPlayer(PlayerController player)
        {
            player.OnItemUseDown += HandleItemUseDown;
            player.OnItemUseUp += HandleItemUseUp;
            DisablePhysics();
            _player = player;
            player.GetComponent<PlayerInventory>().PickUpItem(this);
        }

        private void HandleItemUseDown()
        {
            if (!_equipped) return;
            OnUseButtonDown.Invoke();
        }
        
        private void HandleItemUseUp()
        {
            if (!_equipped) return;
            OnUseButtonUp.Invoke();
        }

        /**
         * remove hooks to the player and deactivate the gameObject
         * used for item switching
         */
        public void UnEquip()
        {
            _equipped = false;
            gameObject.SetActive(false);
        }
        
        /**
         * hooks to the player and activate the gameObject
         * used for item switching
         */
        public void Equip()
        {
            _equipped = true;
            gameObject.SetActive(true);
        }
        
        /**
         * set the parent of this gameObject and reset local position
         */
        public void SetAndMoveToParent(Transform parent)
        {
            _transform.SetParent(parent, false);
            _transform.localPosition = Vector3.zero;
        }

        private void DeregisterFromPlayer()
        {
            if (!_picked) return;
            _player.OnItemUseDown -= HandleItemUseDown;
            _player.OnItemUseUp -= HandleItemUseUp;
            _player.GetComponent<PlayerInventory>().DumpItem(this);
            _player = null;
        }
        
        public void ReturnToPool()
        {
            EnablePhysics();
            _equipped = false;
            _durability = _maxDurability;
            gameObject.SetActive(false);
            _service.UsableItemManager.ReturnUsableItem(_id, this);
            OnReturn.Invoke();
        }
        
        public void IncreaseDurability(int value) => _durability += value;

        public void ReduceDurability(int value)
        {
            _durability -= value;
            if (_durability <= 0)
            {
                OnBreak.Invoke(this);
                DeregisterFromPlayer();
                ReturnToPool();
            }
        }

        public void Spawn(float lifespan)
        {
            _lifespan = lifespan;
        }
    }
}