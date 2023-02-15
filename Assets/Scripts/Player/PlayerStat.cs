using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    public class PlayerStat : MonoBehaviour
    {
        public PlayerID ID { get => _id; }
        public float HealthPercentage => (float) _health / _maxHealth;
            
        [SerializeField] private GameplayService _service;
        [SerializeField] private PlayerID _id;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _killBonus;
        private int _health;
        private PlayerID _lastDamageDealer;

        /**
         * Invoked when the player's lives reaches 0
         * 1st arg: id of the player
         */
        public event UnityAction<int> OnDeath = delegate { }; 
        
        /**
         * Invoked when health of a player change
         * 1st arg: remaining health of the player
         * 2nd arg: max health of the player
         */
        public event UnityAction<PlayerStat> OnHealthChange = delegate { }; 

        private void Awake()
        {
            _health = _maxHealth;
        }

        /**
         * Deduct the health of the player by given damage;
         * set the the player ID of the dealer for kill bonus reference.
         */
        public void DeductHealth(PlayerID lastDealer, DamageInfo damageInfo)
        {
            _health -= damageInfo.Damage;
            _lastDamageDealer = lastDealer;
            OnHealthChange.Invoke(this);
            // Play damage sound only if player has remaining life
            // Otherwise play death SFX
            if (_health > 0)
            {
                _service.AudioManager.PlayAudio(AudioID.Damage);
            }
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (_health > 0) return;
        
            _service.AudioManager.PlayAudio(AudioID.Death);
            // reset the health
            _health = _maxHealth;
            OnHealthChange.Invoke(this);
            // give kill bonus to the last damage dealer
            _service.PlayerManager.IncreaseScore(_lastDamageDealer, _killBonus);
            
            // reduce the remaining life of the player
            _service.PlayerManager.ReduceRemainingLife(ID);
            OnDeath.Invoke(_service.PlayerManager.GetRemainingLife(ID));
        }
    }

    /**
     * A struct containing information of a damage deal
     * Should be used when ever damage is dealt to a player
     */
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