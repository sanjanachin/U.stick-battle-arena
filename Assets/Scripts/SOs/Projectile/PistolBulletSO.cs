using UnityEngine;

namespace Game
{
    /**
     * Data used to generate a PistolBullet gameObject
     */
    [CreateAssetMenu(menuName = "Game/Projectile/PistolBullet", fileName = "PistolBulletSO")]
    public class PistolBulletSO : Reconstructable<PistolBullet>
    {
        public float Gravity { get => _gravity; }
        public float Lifespan { get => _lifespan; }
        public int Damage { get => _damage; }
        public Vector2 Velocity { get => _velocity; }

        [SerializeField] private float _gravity;
        [SerializeField] private float _lifespan;
        [SerializeField] private int _damage;
        [SerializeField] private Vector2 _velocity;
        
        public override PistolBullet Construct(GameObject gameObject)
        {
            PistolBullet bullet = gameObject.AddComponent<PistolBullet>();
            bullet.Initialize(this);
            return bullet;
        }

        public override void Deconstruct(GameObject gameObject)
        {
            Destroy(gameObject.GetComponent<PistolBullet>());
        }
    }
}