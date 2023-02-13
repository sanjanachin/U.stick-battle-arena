using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Projectile))]
    public class PistolBullet : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private float _damage;
        [SerializeField] private float _score;
        
        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _projectile.OnHitPlayer += HandleHitPlayer;
            _projectile.OnHitStage += HandleHitStage;
        }

        private void HandleHitPlayer(PlayerController player, PlayerController executor)
        {
            
            // Increase score of the dealer if hit
            _service.PlayerManager.IncreaseScore(executor.Stat.ID, _score);
            // Deduct health of the hit player
            player.Stat.DeductHealth(executor.Stat.ID, _damage);
            ReturnToPool();
        }
        
        private void ReturnToPool()
        {
            _projectile.ReturnToPool();
        }

        private void HandleHitStage() => ReturnToPool();
    }
}