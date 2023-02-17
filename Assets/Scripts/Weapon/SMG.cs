using System;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class SMG : RangedWeapon
    {
        [SerializeField] private float _fireInterval;
        private float _currTime;
        private bool _shooting;
        private PlayerID _shooter;
        
        private void Awake()
        {
            OnItemUseDown += Shoot;
            OnItemUseUp += Stop;
            OnBreak += Break;
        }

        private void Update()
        {
            if (!_shooting) return;
            
            if (_currTime > 0)
            {
                _currTime -= Time.deltaTime;
            }
            else
            {
                Launch(_shooter);
                _currTime = _fireInterval;
            }
        }

        private void Shoot(PlayerID shooter)
        {
            _shooting = true;
            _shooter = shooter;
            _currTime = 0; // immediate shoot
        }

        private void Stop(PlayerID shooter)
        {
            _shooting = false;
        }

        private void Break()
        {
            _shooting = false;
        }
    }
}