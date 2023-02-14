using System;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class SMG : RangedWeapon
    {
        [SerializeField] private float _fireInterval;
        private float _currTime;
        private bool _shooting;
        private PlayerController _executor;
        
        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.OnUseButtonDown += Shoot;
            _usableItem.OnUseButtonUp += Stop;
            _usableItem.OnBreak += Break;
        }

        private void Update()
        {
            if (!_shooting) return;
            
            if (_currTime > 0)
            {
                _currTime -= Time.deltaTime;
            }
            else
            {
                Projectile bullet = _service.ProjectileManager.
                    SpawnProjectile(_projectileID);

                bullet.transform.position = _shootingPoint.position;

                // flip velocity if facing different direction
                Vector2 velocity = BulletVelocity;
                if (_usableItem.Player.FacingLeft)
                    velocity = new Vector2(-velocity.x, velocity.y);
            
                bullet.Launch(_projectileID, velocity, _executor, BulletGravity, BulletLifespan);
                _usableItem.ReduceDurability(1);

                _currTime = _fireInterval;
            }
        }

        private void Shoot(PlayerController executor)
        {
            _shooting = true;
            _executor = executor;
            _currTime = 0; // immediate shoot
        }

        private void Stop(PlayerController executor)
        {
            _shooting = false;
        }

        private void Break(UsableItem usableItem)
        {
            _shooting = false;
        }
    }
}