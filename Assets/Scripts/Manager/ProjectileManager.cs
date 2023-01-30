using UnityEngine;

namespace Game
{
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        private GameObjectPool<Projectile> _pool;

        private void Awake()
        {
            _pool = new GameObjectPool<Projectile>(_projectilePrefab, transform);
        }

        public Projectile SpawnAndConstruct<T>(Reconstructable<T> projectileData)
        {
            Projectile projectile = _pool.Get((p) =>
            {
                projectileData.Construct(p.gameObject);
            });
            return projectile;
        }

        public void ReturnAndDeconstruct<T>(Reconstructable<T> projectileData, Projectile projectile)
        {
            projectileData.Deconstruct(projectile.gameObject);
            _pool.Release(projectile);
        }
    }
}