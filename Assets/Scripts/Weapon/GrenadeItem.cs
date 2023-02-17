using Game.Player;
using UnityEngine;

namespace Game
{
    public class GrenadeItem : RangedWeapon
    {

        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.IncreaseDurability(1);
            _usableItem.OnUseButtonDown += Throw;
        }

        private void Throw(PlayerController executor)
        {
            Projectile grenade = _service.ProjectileManager.SpawnProjectile(_projectileID);

            grenade.transform.position = _shootingPoint.position;

            Vector2 velocity = BulletVelocity;
            if (_usableItem.Player.FacingLeft)
                velocity = new Vector2(-velocity.x, velocity.y);

            grenade.Launch(_projectileID, velocity, executor, BulletGravity, BulletLifespan);
            _usableItem.ReduceDurability(1);
        }
    }
}