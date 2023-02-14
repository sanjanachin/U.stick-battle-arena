using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(UsableItem))]
    public class Dagger : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private UsableItem _usableItem;
        [SerializeField] private PlayerController _hitPlayer;
        [SerializeField] private float _rayCastDist;
        [SerializeField] private float _rayCastRadius;
        [SerializeField] private float _damage;
        [SerializeField] private float _score;

        private void Awake()
        {
            _usableItem = GetComponent<UsableItem>();
            _usableItem.OnUseButtonDown += Slash;
        }
        
        // Preform a circle hitbox check
        private void Slash(PlayerController executor)
        {
            Vector2 point = (_usableItem.Player.FacingLeft) ? Vector2.left : Vector2.right;
            point = (Vector2) _usableItem.Player.transform.position + (point * _rayCastDist);
            Collider2D target = Physics2D.OverlapCircle(point, _rayCastRadius);
            if (target == null) return;
            
            // check if hit a player
            _hitPlayer = target.GetComponent<PlayerController>();
            if (_hitPlayer != null)
                HandleHit(target.GetComponent<PlayerController>(), executor);
            
            _usableItem.ReduceDurability(1);
        }

        private void HandleHit(PlayerController hit, PlayerController dealer)
        {
            // Increase score of the dealer if hit
            _service.PlayerManager.IncreaseScore(dealer.Stat.ID, _score);
            // Deduct health of the hit player
            hit.Stat.DeductHealth(dealer.Stat.ID, _damage);
        }
    }
}