using Game.Player;
using UnityEngine;

namespace Game
{
    public class Bow : RangedWeapon
    {
        [SerializeField] private float _maxPull;
        [SerializeField] private bool _pulling;
        [SerializeField] private float _pull;

        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.OnUseButtonUp += Release;
            _usableItem.OnUseButtonDown += Pull;
            _usableItem.OnReturn += CancelPull;
            _usableItem.OnSwitchTo += PlaySwitchSound;
        }

        private void Update()
        {
            if (!_pulling) return;

            _pull = Mathf.Min(_pull + Time.deltaTime, _maxPull);
        }

        private void Pull(PlayerController executor)
        {
            _pulling = true;
            _service.AudioManager.PlayAudio(AudioID.BowPull);
        }

        private void CancelPull()
        {
            _pulling = false;

        }
        
        private void Release(PlayerController executor)
        {
            if (!_pulling) return;
            
            Projectile arrow = _service.ProjectileManager.
                SpawnProjectile(_projectileID);

            arrow.transform.position = _shootingPoint.position;

            // flip velocity if facing different direction
            Vector2 velocity = BulletVelocity;
            if (_usableItem.Player.FacingLeft)
                velocity = new Vector2(-velocity.x, velocity.y);
            
            arrow.Launch(_projectileID, _pull * velocity, executor, BulletGravity, BulletLifespan);
            
            _service.AudioManager.PlayAudio(AudioID.BowUse);

            _pulling = false;
            _pull = 0;
            
            _usableItem.ReduceDurability(1);
        }

        private void PlaySwitchSound()
        {
            _service.AudioManager.PlayAudio(AudioID.BowSwitch);
        }
    }
}