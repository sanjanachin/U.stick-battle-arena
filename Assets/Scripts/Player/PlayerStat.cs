using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _killBonus;
        private float _remainingHealth;
        private PlayerID _lastDamageDealer;
    
        public PlayerID ID;

        public event UnityAction OnDeath = delegate { }; 
        public event UnityAction<DamageInfo> OnHealthChange = delegate { }; 

        private void Awake()
        {
            _remainingHealth = _maxHealth;
        }

        private void Update()
        {
            CheckDeath();
        }

        /**
         * Deduct the health of the player by given damage;
         * set the the player ID of the dealer for kill bonus reference.
         */
        public void DeductHealth(float damage, PlayerID lastDealer)
        {
            _remainingHealth -= damage;
            _lastDamageDealer = lastDealer;
        }

        private void CheckDeath()
        {
            if (_remainingHealth > 0) return;
        
            // reset the health
            _remainingHealth = _maxHealth;
            // give kill bonus to the last damage dealer
            _service.PlayerManager.IncreaseScore(_lastDamageDealer, _killBonus);
            // reduce the remaining life of the player
            _service.PlayerManager.ReduceRemainingLife(ID);
            OnDeath.Invoke();
        }
    }

    public struct DamageInfo
    {
        public readonly PlayerID Dealer;
        public readonly PlayerID Target;
        public readonly int Damage;
        public readonly UsableItem ItemUsed;

        public DamageInfo(
            PlayerID dealer, PlayerID target, int damage, UsableItem itemUsed)
        {
            Dealer = dealer;
            Target = target;
            Damage = damage;
            ItemUsed = itemUsed;
        }
    }
}