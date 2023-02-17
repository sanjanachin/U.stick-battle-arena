using Game.Player;
using UnityEngine;

namespace Game
{
    public class SniperBullet : Projectile
    {
        private void Awake()
        {
            OnHitPlayer += HandleHitPlayer;
            OnHitStage += ReturnToPool;
            OnHitProjectile += (_) => ReturnToPool();
        }

        private void HandleHitPlayer(DamageInfo damageInfo)
        {
            if (damageInfo.Dealer == damageInfo.Target) return;

            // Increase score of the dealer if hit
            _service.PlayerManager.IncreaseScore(damageInfo.Dealer, _score);
            // Deduct health of the hit player
            _service.PlayerManager.
                GetPlayerStat(damageInfo.Target).DeductHealth(damageInfo);
            ReturnToPool();
        }
    }
}