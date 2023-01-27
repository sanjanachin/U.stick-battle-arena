using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    public class Dagger : UsableItem
    {
        [SerializeField] private float _rayCastDist;
        [SerializeField] private float _rayCastRadius;

        protected override void Awake()
        {
            base.Awake();
            OnUseButtonDown += Slash;
        }
        
        private void Slash()
        {
            Vector2 point = (_player.FacingLeft) ? Vector2.left : Vector2.right;
            point = (Vector2) _player.transform.position + (point * _rayCastDist);
            Collider2D target = Physics2D.OverlapCircle(point, _rayCastRadius);
            if (target == null) return;
            
            ReduceDurability(1);
        }
    }
}