using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class Buff : MonoBehaviour
    {
        public BuffID ID => _id;
        public int Durability => _durability;
        public float DurabilityPercent => (float) _durability / _maxDurability;
        
        /*
         * Triggered when a buff item is picked up (i.e., collided with a player)
         */
        public event UnityAction OnPickedUp = () => { };

        /*
         * Triggered when a buff finished its count down
         */
        public event UnityAction OnFinished = () => { };

        [SerializeField] protected GameplayService _service;
        [SerializeField] protected AudioID _audioOnPickUp;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Transform _transform;

        [SerializeField] private BuffID _id;
        [SerializeField] private int _maxDurability;
        [SerializeField] private int _durability;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _transform = transform;
        }

        protected abstract void Initialize();
        
        private void Reset()
        {
            _durability = _maxDurability;
            // clear all event calls to prevent left over functions
            // from the previous player, etc
            OnPickedUp = () => { };
            OnFinished = () => { };

            EnablePhysics();
            Initialize();
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

        private void PickUpBy(PlayerController player)
        {
            
        }
    }

}