using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Projectile))]
    public class SMGBullet : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private int _damage;
        [SerializeField] private float _score;
        
        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _projectile.OnHitPlayer += HandleHitPlayer;
            _projectile.OnHitStage += HandleHitStage;
            _projectile.OnHitProjectile += HandleHitProjectile;
        }

        private void HandleHitPlayer(PlayerController target, PlayerController dealer)
        {
            if (target != dealer)
            {
                // Increase score of the dealer if hit
                _service.PlayerManager.IncreaseScore(dealer.Stat.ID, _score);
                // Deduct health of the hit player
                target.Stat.DeductHealth(
                    dealer.Stat.ID, 
                    new DamageInfo(
                        dealer.Stat.ID,
                        target.Stat.ID,
                        _damage,
                        null));
                ReturnToPool();
            }
        }
        
        private void ReturnToPool()
        {
            // added this if statement to prevent multi collisions (e.g., floor and player)
            // that causes release of the same object twice
            if (gameObject.activeSelf)
            {
                _projectile.ReturnToPool();
            }
        }

        private void HandleHitStage() => ReturnToPool();
        
        private void HandleHitProjectile(Projectile other) => ReturnToPool();
    }
}