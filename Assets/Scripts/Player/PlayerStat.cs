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
        public void DeductHealth(PlayerID lastDealer, float damage)
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
}