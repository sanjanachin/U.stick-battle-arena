using System;
using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Projectile))]
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private Pickable _pickable;
        [SerializeField] private float _damage;
        [SerializeField] private float _score;
        [SerializeField] private Transform _visualTransform;
        
        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _pickable = GetComponent<Pickable>();
            _projectile.OnHit += HandleHit;
        }

        private void Update()
        {
            // rotate arrow based on velocity
            Vector2 v = _pickable.Rigidbody.velocity;
            _visualTransform.rotation = Quaternion.AngleAxis(
                Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg, 
                Vector3.forward);
        }

        private void HandleHit(PlayerController player, PlayerController executor)
        {
            // Increase score of the dealer if hit
            _service.PlayerManager.IncreaseScore(executor.Stat.ID, _score);
            // Deduct health of the hit player
            player.Stat.DeductHealth(_damage, executor.Stat.ID);
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