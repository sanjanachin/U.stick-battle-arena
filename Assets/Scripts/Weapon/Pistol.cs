using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(UsableItem))]
    public class Pistol : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private PistolBulletSO _bulletData;
        
        [SerializeField] private UsableItem _usableItem;
        [SerializeField] private Transform _shootingPoint;

        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.OnUseButtonDown += Shoot;
        }
        
        private void Shoot()
        {
            Projectile bullet = _service.ProjectileManager.SpawnAndConstruct(_bulletData);
            bullet.transform.position = _shootingPoint.position;

            Vector2 velocity = _bulletData.Velocity;
            if (_usableItem.Player.FacingLeft)
                velocity = new Vector2(-velocity.x, velocity.y);
            bullet.Launch(velocity, _bulletData.Gravity);
            _usableItem.ReduceDurability(1);
        }
    }
}