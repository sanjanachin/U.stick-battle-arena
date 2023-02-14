using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Projectile))]
    public class PistolBullet : MonoBehaviour
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
        }

        private void HandleHitPlayer(PlayerController target, PlayerController dealer)
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
        
        private void ReturnToPool()
        {
            _projectile.ReturnToPool();
        }

        private void HandleHitStage() => ReturnToPool();
    }
}