using System;
using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    /**
     * Represent a gameObject that can be "pick up" by the player
     * Pickable is considered "picked up" by player when the collider
     * collides with player's collider
     */
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Pickable : MonoBehaviour
    {
        /**
         * Invoked when this collider collides with player's collider
         */
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
            // check if collided with a player
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player == null) return;
            OnPicked.Invoke(player);
        }
    }
}