using System;
using UnityEngine;

namespace Game
{
    public abstract class RangedWeapon : UsableItem
    {
        [SerializeField] protected Transform _shootingPoint;
        [SerializeField] protected Vector2 _velocity;
        [SerializeField] protected float _gravity;
        
        [SerializeField] private ProjectileID _projectileID;
        
        protected void Launch(PlayerID shooter)
        {
            LaunchInfo launchInfo = new LaunchInfo(
                _shootingPoint.position,
                _velocity * _shootingPoint.localScale.x,
                _gravity,
                shooter
            );

            Launch(shooter, launchInfo);
        }
        
        protected void Launch(PlayerID shooter, LaunchInfo launchInfo)
        {
            _service.ProjectileManager.SpawnAndLaunch(_projectileID, launchInfo);
            _service.AudioManager.PlayAudio(_audioOnUse);
            
            ReduceDurability(1);
        }
        
        public struct LaunchInfo
        {
            public Vector3 Origin { get; private set; }
            public Vector2 Velocity { get; private set; }
            public float Gravity { get; private set; }
            public PlayerID Shooter { get; private set; }

            public LaunchInfo(Vector3 origin, Vector2 velocity, float gravity, PlayerID shooter)
            {
                Origin = origin;
                Velocity = velocity;
                Gravity = gravity;
                Shooter = shooter;
            }
        }
    }
}