using Game.Player;
using UnityEngine;

namespace Game
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        public PlayerStat PlayerStatStat => _playerStat;
        
        [SerializeField] private GameplayService _service;
        [SerializeField] private PlayerID _playerID;
        private PlayerStat _playerStat;

        public void Initialize(PlayerStat playerStat)
        {
            _playerStat = playerStat;
            _playerStat.transform.position = transform.position;
            _playerStat.OnDeath += Respawn;
        }

        /**
         * Change the position of the player to the position of the spawner
         */
        private void Respawn(PlayerID _)
        {
            _playerStat.transform.position = transform.position;
        }
    }
}
