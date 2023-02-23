using UnityEngine;

namespace Game
{
    public class Shotgun : RangedWeapon
    {
        // total number of bullets in one shot, recommended is 3 or 4
        [SerializeField] private int _bulletNumber;
        // for creating spread of shotgun bullets
        [SerializeField] private float _yOffset;
        
        protected override void Initialize()
        {
            OnItemUseDown += Shoot;
        }
        
        private void Shoot(PlayerID shooter)
        {
            // Initial bullet if _bulletNumber is odd
            if (_bulletNumber % 2 != 0)
                ShootOneBullet(shooter, 0f);

            for (int i = 1; i <= _bulletNumber / 2; i++)
            {
                ShootOneBullet(shooter, _yOffset * i);
                ShootOneBullet(shooter, -_yOffset * i);
            }
        }

        private void ShootOneBullet(PlayerID shooter, float yVelocity)
        {
            LaunchInfo launchInfo = new LaunchInfo(
                _shootingPoint.position,
                VelocityWithFlip(new Vector2(_velocity.x, yVelocity)),
                _gravity,
                shooter
            );
            
            Launch(shooter, launchInfo);
        }
    }
}