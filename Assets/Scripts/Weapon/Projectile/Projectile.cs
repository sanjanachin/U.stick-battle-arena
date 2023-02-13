using System;
using UnityEngine;
using UnityEngine.Events;
using Game.Player;

namespace Game
{
    /**
     * Represent a projectile that is launch and travels with
     * certain velocity and gravity
     */
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        /**
         * Invoked on launched
         */
        public event UnityAction OnLaunch = delegate { };
        /**
         * Invoked on hitting a player
         */
        // TODO: make <PlayerController, PlayerController> a struct with damage/attack detail
        public event UnityAction<PlayerController, PlayerController> OnHitPlayer = delegate { };
        public event UnityAction OnHitStage = delegate { };
        
        public Vector2 Velocity => _rigidbody.velocity;
        public float Gravity => _rigidbody.gravityScale;

        [SerializeField] private GameplayService _service;
        [SerializeField] private float _lifespan;
        [SerializeField] private ProjectileID _id;

        private PlayerController _executor;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            if (!isActiveAndEnabled) return;

            // countdown and destroy bullet 
            if (_lifespan > 0)
            {
                _lifespan -= Time.deltaTime;
            } 
            else 
            {
                ReturnToPool();
            }
        }

        /**
         * Launch the projectile with provided values
         */
        public void Launch(ProjectileID id, Vector2 velocity, PlayerController executor, float gravity, float lifespan)
        {
            _executor = executor;
            _rigidbody.velocity = velocity;
            _rigidbody.gravityScale = gravity;
            _lifespan = lifespan;
            _id = id;
            
            OnLaunch.Invoke();
        }

        /**
         * Return this Object to the pool
         */
        public void ReturnToPool()
        {
            _service.ProjectileManager.ReturnProjectile(_id, this);
        }

        // called on the projectile collides with a collider
        private void Hit(PlayerController player)
        {
            OnHitPlayer.Invoke(player, _executor);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!enabled) return;
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player == null)
            {
                OnHitStage.Invoke();
                return;
            }
            
            Hit(player);
        }
    }
}