using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    public class PlayerStat : MonoBehaviour
    {
        public PlayerID ID { get => _id; }

        [SerializeField] private GameplayService _service;
        [SerializeField] private PlayerID _id;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _killBonus;
        private int _remainingHealth;
        private PlayerID _lastDamageDealer;


        public event UnityAction<PlayerID> OnDeath = delegate { }; 
        public event UnityAction<int, int> OnHealthChange = delegate { }; 

        private void Awake()
        {
            _remainingHealth = _maxHealth;
        }

        /**
         * Deduct the health of the player by given damage;
         * set the the player ID of the dealer for kill bonus reference.
         */
        public void DeductHealth(PlayerID lastDealer, DamageInfo damageInfo)
        {
            _remainingHealth -= damageInfo.Damage;
            _lastDamageDealer = lastDealer;
            OnHealthChange.Invoke(_remainingHealth, _maxHealth);
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (_remainingHealth > 0) return;
        
            // reset the health
            _remainingHealth = _maxHealth;
            OnHealthChange.Invoke(_remainingHealth, _maxHealth);
            
            // give kill bonus to the last damage dealer
            _service.PlayerManager.IncreaseScore(_lastDamageDealer, _killBonus);
            
            // reduce the remaining life of the player
            _service.PlayerManager.ReduceRemainingLife(ID);
            OnDeath.Invoke(_id);
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