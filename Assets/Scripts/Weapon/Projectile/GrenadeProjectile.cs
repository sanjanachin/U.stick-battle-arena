using System;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class GrenadeProjectile : Projectile
    {
        [SerializeField] private float _splashRadius;
        
        private void Awake()
        {
            OnHitPlayer += HandleHitPlayer;
            OnHitStage += Explode;
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
        
        private void Explode()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _splashRadius);
            foreach (Collider2D hit in hits)
            {
                PlayerStat player = hit.GetComponent<PlayerStat>();
                if (player != null) continue;
                
                DamageInfo damageInfo = CreateDamageInfo(player.ID);
                // Deduct health of the player
                _service.PlayerManager.
                    GetPlayerStat(damageInfo.Target).DeductHealth(damageInfo);
            }
            ReturnToPool();
        }
    }
}