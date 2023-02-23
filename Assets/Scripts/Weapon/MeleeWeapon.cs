using System.Collections;
using Game.Player;
using UnityEngine;

namespace Game
{
    public abstract class MeleeWeapon : UsableItem
    {
        public int Damage => _damage;
        public bool IsAvailable = true;
        [SerializeField] private float CooldownDuration;
        [SerializeField] private int _damage;
        [SerializeField] private Vector2 _knockbackDirection;
        [SerializeField] private float _knockback;
        [SerializeField] private float _rayCastRadius;
        [SerializeField] private Transform _hitBoxOrigin;

        protected virtual void Attack(PlayerID attacker)
        {
            if (IsAvailable == false)
            {
                return;
            }
            Collider2D col = Physics2D.OverlapCircle(_hitBoxOrigin.position, _rayCastRadius);
            if (col == null) return;
            
            // check if hit a player
            PlayerStat target = col.GetComponent<PlayerStat>();
            if (target != null)
            {
                // Deduct health of the hit player
                target.DeductHealth(
                    new DamageInfo(
                        attacker,
                        target.ID,
                        _damage,
                        this));
                Vector2 tempKnockback = new Vector2(_knockbackDirection.x * transform.parent.localScale.x * -1,
                    _knockbackDirection.y);
                Rigidbody2D playerBody = target.GetComponent<Rigidbody2D>();
                playerBody.velocity += tempKnockback * _knockback;

            }
            
            _service.AudioManager.PlayAudio(_audioOnUse);
            ReduceDurability(1);
            StartCoroutine((StartCooldown()));
        }
        
        public IEnumerator StartCooldown()
        {
            IsAvailable = false;
            yield return new WaitForSeconds(CooldownDuration);
            IsAvailable = true;
        }
    }
}