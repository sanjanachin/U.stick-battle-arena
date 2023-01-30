using System;
using System.Threading;
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
            _projectile.OnLaunch += ReturnCountdown;
        }
        
        public void Initialize(PistolBulletSO data)
        {
            _data = data;
        }
        
        private void Update()
        {
            if (_lifespan > 0)
            {
                _lifespan -= Time.deltaTime;
            } 
            else 
            {
                ReturnToPool();
            }
        }

        private void ReturnCountdown()
        {
            _lifespan = _data.Lifespan;
        }

        private void HandleHit(PlayerController player)
        {
            Debug.Log($"deal {_data.Damage} to player");
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            _projectile.OnHit -= HandleHit;
            _projectile.OnLaunch -= ReturnCountdown;
            _projectile.ReturnToPool(_data);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player != null) return;
            
            // hits a wall / floor
            ReturnToPool();
        }
    }
}