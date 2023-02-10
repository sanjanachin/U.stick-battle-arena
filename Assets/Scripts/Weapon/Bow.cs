using UnityEngine;

namespace Game
{
    public class Bow : RangedWeapon
    {
        [SerializeField] private float _maxPull;
        [SerializeField] private bool _pulling;
        [SerializeField] private float _pull;

        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.OnUseButtonUp += Release;
            _usableItem.OnUseButtonDown += Pull;
            // _usableItem.OnReturn += CancelPull;
        }

        private void Update()
        {
            if (!_pulling) return;

            _pull = Mathf.Min(_pull + Time.deltaTime, _maxPull);
        }

        private void Pull()
        {
            _pulling = true;
        }

        private void CancelPull()
        {
            _pulling = false;

        }
        
        private void Release()
        {
            if (!_pulling) return;
            
            Projectile arrow = _service.ProjectileManager.
                SpawnProjectile(_projectileID);

            arrow.transform.position = _shootingPoint.position;

            // flip velocity if facing different direction
            Vector2 velocity = BulletVelocity;
            if (_usableItem.Player.FacingLeft)
                velocity = new Vector2(-velocity.x, velocity.y);
            
            arrow.Launch(_projectileID, _pull * velocity, BulletGravity, BulletLifespan);
            _pulling = false;
            _pull = 0;
            
            _usableItem.ReduceDurability(1);
        }
    }
}