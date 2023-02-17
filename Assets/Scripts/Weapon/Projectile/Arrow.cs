using System;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class Arrow : Projectile
    {
        [SerializeField] private Transform _visualTransform;
        
        private void Awake()
        {
            OnHitPlayer += HandleHitPlayer;
            OnHitStage += ReturnToPool;
            OnHitProjectile += (_) => ReturnToPool();
        }

        private void Update()
        {
            // rotate arrow based on velocity
            Vector2 v = Rigidbody.velocity;
            _visualTransform.rotation = Quaternion.AngleAxis(
                Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg, 
                Vector3.forward);
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