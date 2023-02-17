using Game.Player;
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
            OnItemUseDown += Release;
            OnItemUseUp += (_) => Pull();
            OnReturn += CancelPull;
            OnHold += (_) => CancelPull();
        }

        private void Update()
        {
            if (!_pulling) return;

            _pull = Mathf.Min(_pull + Time.deltaTime, _maxPull);
        }

        private void Pull()
        {
            _pulling = true;
            _service.AudioManager.PlayAudio(AudioID.BowPull);
        }

        private void CancelPull()
        {
            _pulling = false;
        }
        
        private void Release(PlayerID shooter)
        {
            if (!_pulling) return;
            Launch(shooter);
            
            _service.AudioManager.PlayAudio(AudioID.BowUse);

            _pulling = false;
            _pull = 0;
        }
    }
}