using System;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class Shotgun : RangedWeapon
    {
        // total number of bullets in one shot, recommended is 3 or 4
        [SerializeField] private int _bulletNumber;
        // for creating spread of shotgun bullets
        [SerializeField] private float _yOffset;
        
        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.OnUseButtonDown += Shoot;
            _usableItem.OnSwitchTo += PlaySwitchSound;
        }
        
        private void Shoot(PlayerController executor)
        {
            // Initial bullet if _bulletNumber is odd
            if (_bulletNumber % 2 != 0)
                ShootOneBullet(executor, 0f);

            for (int i = 1; i <= _bulletNumber / 2; i++)
            {
                ShootOneBullet(executor, _yOffset * i);
                ShootOneBullet(executor, -_yOffset * i);
            }
            
            _service.AudioManager.PlayAudio(AudioID.ShotgunShoot);
            
            _usableItem.ReduceDurability(1);
        }

        private void ShootOneBullet(PlayerController executor, float yVelocity)
        {
            
            Projectile bullet = _service.ProjectileManager.
                SpawnProjectile(_projectileID);

            bullet.transform.position = _shootingPoint.position;

            // flip velocity if facing different direction
            Vector2 velocity = new Vector2(BulletVelocity.x, yVelocity);
            if (_usableItem.Player.FacingLeft)
                velocity = new Vector2(-velocity.x, velocity.y);
            
            bullet.Launch(_projectileID, velocity, executor, BulletGravity, BulletLifespan);
        }

        private void PlaySwitchSound()
        {
            _service.AudioManager.PlayAudio(AudioID.ShotgunRack);
        }
    }
}