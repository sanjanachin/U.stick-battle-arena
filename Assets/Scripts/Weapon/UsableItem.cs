#region

using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Game
{
    /**
     * Represent a item that can be used by the player using the
     * "use" button
     * Rely on Pickable component to be pick up by a player and to be used
     * Every item and weapon on stage should inherit this class and operate on
     * the provided events calls.
     * The derived class should override the Initialize function to hook up events as needed
     */
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class UsableItem : MonoBehaviour
    {
        public UsableItemID ID => _id;
        public Sprite Icon => _icon;
        public int Durability => _durability;
        public float DurabilityPercent => (float) _durability / _maxDurability;
        
        /**
         * Invoked when this item is picked up
         */
        public event UnityAction OnPickedUp = () => { };
        /**
         * Invoked when the item use button on this item is down
         */
        public event UnityAction<PlayerID> OnItemUseDown = (_) => { };
        /**
         * Invoked when the item use button on this item is up
         */
        public event UnityAction<PlayerID> OnItemUseUp = (_) => { };
        /**
         * Invoked when this item is held by the player, which is switched into the inventory
         */
        public event UnityAction<PlayerID> OnHold = (_) => { };
        /**
         * Invoked when this item is equipped by the player
         * either when picked up or switched out of the inventory for use
         */
        public event UnityAction<PlayerID> OnEquip = (_) => { };
        /**
         * Invoked when the durability has changed
         */
        public event UnityAction<UsableItem> OnDurabilityChange = (_) => { };
        /**
         * Invoked when this item is returned to the pool
         */
        public event UnityAction OnReturn = () => { };
        /**
         * Invoked when this item breaks, which is when durability <= 0
         */
        public event UnityAction<UsableItem> OnBreak = (_) => { };

        [SerializeField] protected GameplayService _service;
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected GameObject _visual;
        [SerializeField] protected AudioID _audioOnUse;
        [SerializeField] protected AudioID _audioOnEquip;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Transform _transform;

        [SerializeField] private UsableItemID _id;
        [SerializeField] private int _maxDurability;
        [SerializeField] private int _durability;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _transform = transform;

            OnHold += (_) => gameObject.SetActive(false);
            OnEquip += (_) => gameObject.SetActive(true);
            
            Reset();
        }

        /**
         * Every derived class should override this function to
         * set up the needed event calls for its unique behavior
         */
        protected abstract void Initialize();

        private void Reset()
        {
            _durability = _maxDurability;
            // clear all event calls to prevent left over functions
            // from the previous player, etc
            OnPickedUp = () => { };
            OnItemUseDown = (_) => { };
            OnItemUseUp = (_) => { };
            OnHold = (_) => { };
            OnEquip = (_) => { };
            OnReturn = () => { };
            OnDurabilityChange = (_) => { };
            OnBreak = (_) => { };

            EnablePhysics();
            Initialize();
        }

        /**
         * set the parent of this gameObject and reset local position
         */
        public void SetAndMoveToParent(Transform parent)
        {
            _transform.SetParent(parent, false);
            _transform.localPosition = Vector3.zero;
        }
        
        private void DisablePhysics()
        {
            _collider.enabled = false;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.isKinematic = true;
        }
        
        private void EnablePhysics()
        {
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
        }

        /**
         * called by the player inventory when the item use is down
         * to trigger the event
         */
        public void ItemUseDown(PlayerID itemUser) => OnItemUseDown.Invoke(itemUser);
        
        /**
         * called by the player inventory when the item is held
         * to trigger the event
         */
        public void Hold(PlayerID itemUser) => OnHold.Invoke(itemUser);

        /**
         * called by the player inventory when the item is equipped
         * to trigger the event
         */
        public void Equip(PlayerID itemUser)
        {
            _service.AudioManager.PlayAudio(_audioOnEquip);
            OnEquip.Invoke(itemUser);
        }
        
        /**
         * called by the player inventory when the item use is up
         * to trigger the event
         */
        public void ItemUseUp(PlayerID itemUser) => OnItemUseUp.Invoke(itemUser);

        /**
         * called by the player inventory to set the item object
         * state to attach to the player object
         */
        public void PickUpBy(Transform itemHolder)
        {
            DisablePhysics();
            SetAndMoveToParent(itemHolder);
            OnPickedUp.Invoke();
        }

        /**
         * Reduce the durability of the item by the given value
         */
        public void ReduceDurability(int value)
        {
            _durability -= value;
            OnDurabilityChange.Invoke(this);
            if (_durability > 0) return;
            OnBreak.Invoke(this);
            OnReturn.Invoke();
            _service.UsableItemManager.ReturnUsableItem(_id, this);
            Reset();
        }

        public void MakeInvisible()
        {
            _visual.SetActive(false);
        }

        public void MakeVisible()
        {
            _visual.SetActive(true);
        }
    }
}