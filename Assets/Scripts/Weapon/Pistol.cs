using UnityEngine;

namespace Game
{
    public class Pistol : RangedWeapon
    {
        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.OnUseButtonDown += Shoot;
        }
        
        private void Shoot()
        {
            Projectile bullet = _service.ProjectileManager.
                SpawnProjectile(_projectileID);

            bullet.transform.position = _shootingPoint.position;

            // flip velocity if facing different direction
            Vector2 velocity = BulletVelocity;
            if (_usableItem.Player.FacingLeft)
                velocity = new Vector2(-velocity.x, velocity.y);
            
            bullet.Launch(_projectileID, velocity, BulletGravity, BulletLifespan);
            _usableItem.ReduceDurability(1);
        }
    }
}