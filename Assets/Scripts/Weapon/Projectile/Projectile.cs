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
    public abstract class Projectile : MonoBehaviour
    {
        public event UnityAction OnHitStage = () => { };
        public event UnityAction<Projectile> OnHitProjectile = (_) => { };
        public event UnityAction<DamageInfo> OnHitPlayer = (_) => { };
        public event UnityAction OnLaunch = () => { };

        public Rigidbody2D Rigidbody => _rigidbody;
        
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Transform _transform;
        private PlayerID _shooter;

        [SerializeField] protected GameplayService _service;
        
        [SerializeField] protected int _damage;
        [SerializeField] protected int _score;
        [SerializeField] protected ProjectileID _id;

        [SerializeField] private bool _hit = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _transform = transform;
        }

        public void Launch(Vector2 velocity, float gravity, PlayerID shooter)
        {
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

        public DamageInfo CreateDamageInfo(PlayerID target)
        {
            return new DamageInfo(
                _shooter,
                target,
                _damage,
                this);
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
    }
}