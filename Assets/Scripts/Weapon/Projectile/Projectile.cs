using UnityEngine;
using UnityEngine.Events;
using Game.Player;

namespace Game
{
    /**
     * Represent a projectile that is launch and travels with
     * certain velocity and gravity
     */
    [RequireComponent(typeof(Pickable))]
    public class Projectile : MonoBehaviour
    {
        /**
         * Invoked on launched
         */
        public event UnityAction OnLaunch = delegate { };
        /**
         * Invoked on hitting a player
         */
        public event UnityAction<PlayerController> OnHit = delegate { };
        
        public Vector2 Velocity => _pickable.Rigidbody.velocity;
        public float Gravity => _pickable.Rigidbody.gravityScale;

        [SerializeField] private GameplayService _service;
        [SerializeField] private Pickable _pickable;
        [SerializeField] private float _lifespan;
        [SerializeField] private ProjectileID _id;
        
        private void Awake()
        {
            _pickable = GetComponent<Pickable>();
            _pickable.OnPicked += Hit;
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
        public void Launch(ProjectileID id, Vector2 velocity, float gravity, float lifespan)
        {
            _pickable.Rigidbody.velocity = velocity;
            _pickable.Rigidbody.gravityScale = gravity;
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
            OnHit.Invoke(player);
        }
    }
}