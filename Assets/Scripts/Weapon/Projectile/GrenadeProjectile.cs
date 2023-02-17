using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Projectile))]
    public class GrenadeProjectile : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private int _damage;
        [SerializeField] private float _score;
        [SerializeField] private float _splashRadius;
        
        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _projectile.OnHitPlayer += HandleHitPlayer;
            _projectile.OnHitStage += HandleHitStage;
        }

        private void HandleHitPlayer(PlayerController target, PlayerController dealer)
        {
            Debug.Log($"Hit {target}");
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
            Explode(dealer.Stat.ID);
        }

         private void HandleHitStage() => ReturnToPool();

        private void Explode(PlayerID throwerID)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _splashRadius);
            foreach (Collider2D hit in hits)
            {
                PlayerController player = hit.GetComponent<PlayerController>();
                if (player != null)
                {
                    // Deduct health of the player
                    player.Stat.DeductHealth(
                        throwerID, 
                        new DamageInfo(
                            throwerID,
                            player.Stat.ID,
                            _damage,
                            null));
                }
            }
            ReturnToPool();
        }
        
        private void ReturnToPool()
        {
            _projectile.ReturnToPool();
        }

    }
}