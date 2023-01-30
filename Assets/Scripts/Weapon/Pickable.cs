using System;
using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Pickable : MonoBehaviour
    {
        public event UnityAction<PlayerController> OnPicked = delegate {  };
        
        public Collider2D Collider { get => _collider; }
        public Rigidbody2D Rigidbody { get => _rigidbody; }
        
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            // check if collided with a usable item
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player == null) return;
            OnPicked.Invoke(player);
        }
    }
}