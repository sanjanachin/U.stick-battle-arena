using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(UsableItem))]
    public class Dagger : MonoBehaviour
    {
        [SerializeField] private UsableItem _usableItem;
        [SerializeField] private float _rayCastDist;
        [SerializeField] private float _rayCastRadius;

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
            
            _usableItem.ReduceDurability(1);
        }
    }
}