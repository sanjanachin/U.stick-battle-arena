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
        
        private void Awake()
        {
            _pickable = GetComponent<Pickable>();
            _pickable.OnPicked += Hit;
        }

        /**
         * Launch the projectile with provided values
         */
        public void Launch(Vector2 velocity, float gravity)
        {
            _pickable.Rigidbody.velocity = velocity;
            _pickable.Rigidbody.gravityScale = gravity;
            
            OnLaunch.Invoke();
        }

        /**
         * Deconstruct and return this Object to the pool
         */
        public void ReturnToPool<T>(Reconstructable<T> data)
        {
            _service.ProjectileManager.ReturnAndDeconstruct(data, this);
        }

        // called on the projectile collides with a collider
        private void Hit(PlayerController player)
        {
            OnHit.Invoke(player);
        }
    }
}