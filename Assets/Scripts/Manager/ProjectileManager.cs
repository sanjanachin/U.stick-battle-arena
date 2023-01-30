using UnityEngine;

namespace Game
{
    /**
     * Manage all the projectiles on scene
     */
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        private GameObjectPool<Projectile> _pool;

        private void Awake()
        {
            _pool = new GameObjectPool<Projectile>(_projectilePrefab, transform);
        }

        /**
         * Get a projectile prefab from the pool and let the given
         * Reconstructable to construct the gameObject
         */
        public Projectile SpawnAndConstruct<T>(Reconstructable<T> projectileData)
        {
            Projectile projectile = _pool.Get((p) =>
            {
                projectileData.Construct(p.gameObject);
            });
            return projectile;
        }
        
        /**
         * Deconstruct and return the projectile gameObject to the pool
         */
        public void ReturnAndDeconstruct<T>(Reconstructable<T> projectileData, Projectile projectile)
        {
            projectileData.Deconstruct(projectile.gameObject);
            _pool.Release(projectile);
        }
    }
}