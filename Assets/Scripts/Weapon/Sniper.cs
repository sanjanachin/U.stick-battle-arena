using Game.Player;
using UnityEngine;

namespace Game
{
    public class Sniper : RangedWeapon
    {
        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.OnUseButtonDown += Shoot;
        }
        
        private void Shoot(PlayerController executor)
        {
            Projectile bullet = _service.ProjectileManager.
                SpawnProjectile(_projectileID);

            bullet.transform.position = _shootingPoint.position;

            // flip velocity if facing different direction
            Vector2 velocity = BulletVelocity;
            if (_usableItem.Player.FacingLeft)
                velocity = new Vector2(-velocity.x, velocity.y);
            
            bullet.Launch(_projectileID, velocity, executor, BulletGravity, BulletLifespan);
            _usableItem.ReduceDurability(1);
        }
    }
}