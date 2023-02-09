using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Projectile))]
    public class PistolBullet : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private float _damage;
        [SerializeField] private float _score;
        
        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _projectile.OnHit += HandleHit;
        }

        private void HandleHit(PlayerController player, PlayerController executor)
        {
            
            _service.PlayerManager.IncreaseScore(executor.Stat.PlayerID, _score);
            player.Stat.DeductHealth(_damage, executor.Stat.PlayerID);
            ReturnToPool();
        }
        
        private void ReturnToPool()
        {
            _projectile.ReturnToPool();
        }
        
        // check for wall / floor hits
        private void OnCollisionEnter2D(Collision2D col)
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player != null) return;
            
            // hits a wall / floor
            ReturnToPool();
        }
    }
}