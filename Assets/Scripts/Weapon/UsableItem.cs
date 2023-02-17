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
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class UsableItem : MonoBehaviour
    {
        public UsableItemID ID => _id;
        public Sprite Icon => _icon;
        public int Durability => _durability;
        public float DurabilityPercent => (float) _durability / _maxDurability;
        
        public event UnityAction OnPickedUp = () => { };
        public event UnityAction<PlayerID> OnItemUseDown = (_) => { };
        public event UnityAction<PlayerID> OnItemUseUp = (_) => { };
        public event UnityAction<PlayerID> OnHold = (_) => { };
        public event UnityAction<PlayerID> OnEquip = (_) => { };
        public event UnityAction<UsableItem> OnDurabilityChange = (_) => { };
        public event UnityAction OnReturn = () => { };
        public event UnityAction OnBreak = () => { };

        [SerializeField] protected GameplayService _service;
        [SerializeField] protected Sprite _icon;
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
        }

        private void Reset()
        {
            _durability = _maxDurability;
            OnPickedUp = () => { };
            OnItemUseDown = (_) => { };
            OnItemUseUp = (_) => { };
            OnHold = (_) => { };
            OnEquip = (_) => { };
            OnReturn = () => { };
            OnDurabilityChange = (_) => { };
            OnBreak = () => { };

            EnablePhysics();
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

        public void ItemUseDown(PlayerID itemUser) => OnItemUseDown.Invoke(itemUser);
        public void Hold(PlayerID itemUser) => OnHold.Invoke(itemUser);

        public void Equip(PlayerID itemUser)
        {
            _service.AudioManager.PlayAudio(_audioOnEquip);
            OnEquip.Invoke(itemUser);
        }
        
        public void ItemUseUp(PlayerID itemUser) => OnItemUseUp.Invoke(itemUser);

        public void PickUpBy(Transform itemHolder)
        {
            DisablePhysics();
            SetAndMoveToParent(itemHolder);
            OnPickedUp.Invoke();
        }

        public void ReduceDurability(int value)
        {
            _durability -= value;
            OnDurabilityChange.Invoke(this);
            if (_durability > 0) return;
            OnBreak.Invoke();
            OnReturn.Invoke();
            _service.UsableItemManager.ReturnUsableItem(_id, null);
            Reset();
        }
    }
}