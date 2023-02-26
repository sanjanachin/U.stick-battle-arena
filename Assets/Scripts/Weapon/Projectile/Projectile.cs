#region

using Game.Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

#endregion

namespace Game
{
    /**
     * Represent a projectile that is launch and travels with
     * certain velocity and gravity
     * Every projectile on stage should inherit this class and operate on
     * the provided events calls when needed.
     * The derived class should override the Start function to hook up events as needed
     */
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public abstract class Projectile : MonoBehaviour
    {
        /**
         * Invoked when the projectile hits the stage
         */
        public event UnityAction OnHitStage = () => { };
        /**
         * Invoked when the projectile hits another projectile
         */
        public event UnityAction<Projectile> OnHitProjectile = (_) => { };
        /**
         * Invoked when the projectile hits a player
         */
        public event UnityAction<DamageInfo> OnHitPlayer = (_) => { };
        /**
         * Invoked when the projectile is launched
         */
        public event UnityAction OnLaunch = () => { };

        public Rigidbody2D Rigidbody => _rigidbody;
        
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Transform _transform;
        private PlayerID _shooter;

        [SerializeField] protected GameplayService _service;

        [SerializeField] protected ProjectileID _id;
        [SerializeField] protected int _damage;
        [SerializeField] protected int _score;
        [SerializeField] protected bool _hit = false;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _transform = transform;
        }

        /**
         * Apply the given velocity and gravity to the projectile while
         * keeping track of the shooter
         */
        public void Launch(Vector2 velocity, float gravity, PlayerID shooter)
        {
            #if UNITY_EDITOR_WIN
                Assert.IsTrue(_hit == false);
            #endif
            Assert.IsTrue(_hit == false);
            _shooter = shooter;
            _rigidbody.velocity = velocity;
            _rigidbody.gravityScale = gravity;
            
            OnLaunch.Invoke();
        }

        // reset the value to initial value
        // potentially need to reset events depend on later implementations
        private void Reset()
        {
            _hit = false;
        }
        
        /**
         * Return this Object to the pool
         */
        public void ReturnToPool()
        {
            _service.ProjectileManager.ReturnProjectile(_id, this);
            Reset();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // if already hit something, ignore rest of the collision
            if (_hit) return;
            if (!isActiveAndEnabled) return;
            _hit = true;

            PlayerStat target = col.gameObject.GetComponent<PlayerStat>();
            if (target != null) OnHitPlayer.Invoke(CreateDamageInfo(target.ID));
 
            Projectile projectile = col.gameObject.GetComponent<Projectile>();
            if (projectile != null) OnHitProjectile.Invoke(projectile);

            if (target == null && projectile == null) OnHitStage.Invoke();
        }
        
        /**
         * Creates a DamageInfo struct as if this projectile dealt damage to
         * the target player.
         */
        public DamageInfo CreateDamageInfo(PlayerID target)
        {
            return new DamageInfo(
                _shooter,
                target,
                _damage,
                this);
        }
    }
}