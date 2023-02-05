using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(UsableItem))]
    public class RangedWeapon : MonoBehaviour
    {
        public float BulletGravity { get => _bulletGravity; }
        public float BulletLifespan { get => _bulletLifespan; }
        public Vector2 BulletVelocity { get => _bulletVelocity; }
        
        [SerializeField] protected GameplayService _service;
        
        [SerializeField] protected UsableItem _usableItem;
        [SerializeField] protected Transform _shootingPoint;
        [SerializeField] protected ProjectileID _projectileID;

        [Header("Shooting Settings")]
        [SerializeField] private Vector2 _bulletVelocity;
        [SerializeField] private float _bulletGravity;
        [SerializeField] private float _bulletLifespan;
    }
}