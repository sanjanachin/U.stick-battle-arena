using Game.Player;
using UnityEngine;

namespace Game
{
    public class ShotgunBullet : Projectile
    {
        private void Start()
        {
            OnHitPlayer += HandleHitPlayer;
            OnHitStage += ReturnToPool;
            OnHitProjectile += HandleHitProjectile;
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

        private void HandleHitStage() => ReturnToPool();

        private void HandleHitProjectile(Projectile other)
        {
            // does not return to pool if collide with shotgun bullets
            if (other.gameObject.GetComponent<ShotgunBullet>() == null)
                ReturnToPool();
        }
    }
}