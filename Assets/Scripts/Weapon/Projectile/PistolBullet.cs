using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Projectile))]
    public class PistolBullet : MonoBehaviour
    {
        [SerializeField] private PistolBulletSO _data;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private float _lifespan;
        
        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _projectile.OnHit += HandleHit;
            _projectile.OnLaunch += StartReturnCountdown;
        }
        
        /**
         * Initialize the gameObject
         * Should ONLY be called by corresponding Reconstructable
         */
        public void Initialize(PistolBulletSO data)
        {
            _data = data;
        }
        
        private void Update()
        {
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

        private void StartReturnCountdown()
        {
            // reset countdown timer
            _lifespan = _data.Lifespan;
        }

        private void HandleHit(PlayerController player)
        {
            Debug.Log($"deal {_data.Damage} to player");
            ReturnToPool();
        }
        
        /**
         * Reset hooks before deconstruction
         */
        private void ReturnToPool()
        {
            _projectile.OnHit -= HandleHit;
            _projectile.OnLaunch -= StartReturnCountdown;
            _projectile.ReturnToPool(_data);
        }
        
        // check for wall / floor hits
        private void OnCollisionEnter2D(Collision2D col)
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player != null) return;
            
            // hits a wall / floor
            ReturnToPool();
        }
    }
}