using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectilePool = Game.GameObjectPool<Game.Projectile>;

namespace Game
{
    public enum ProjectileID
    {
        PistolBullet = 1,
        Arrow = 2,
        SniperBullet = 3,
        SMGBullet = 4,
        ShotgunBullet = 5,
        Grenade = 6,
    }
    
    /**
     * Manage all the projectiles on scene
     */
    public class ProjectileManager : MonoBehaviour
    {
        private Dictionary<ProjectileID, ProjectilePool> _poolMap;

        [SerializeField]  private ProjectileIdPrefabPair[] _prefabEntries;

        private void Awake()
        {
            _poolMap = new Dictionary<ProjectileID, ProjectilePool>();

            for (int i = 0; i < _prefabEntries.Length; i++)
            {
                Transform spawnParent = new GameObject(
                    $"{_prefabEntries[i].Id} Pool").GetComponent<Transform>();
                spawnParent.SetParent(transform);
                
                _poolMap.Add(
                    _prefabEntries[i].Id,
                    new ProjectilePool(_prefabEntries[i].Prefab, spawnParent));
            }
        }

        /**
         * Get a projectile prefab from the pool and let the given
         */
        public Projectile SpawnAndLaunch(ProjectileID id, RangedWeapon.LaunchInfo launchInfo)
        {
            ProjectilePool pool = _poolMap[id];
            return pool.Get((projectile) => { 
                projectile.transform.position = launchInfo.Origin;
                projectile.Launch(launchInfo.Velocity, launchInfo.Gravity, launchInfo.Shooter);
            });
        }
        
        /**
         * Return the projectile gameObject to the pool
         */
        public void ReturnProjectile(ProjectileID id, Projectile projectile)
        {
            _poolMap[id].Release(projectile);
        }

        [Serializable]
        private struct ProjectileIdPrefabPair
        {
            public ProjectileID Id;
            public Projectile Prefab;
        }
    }
}