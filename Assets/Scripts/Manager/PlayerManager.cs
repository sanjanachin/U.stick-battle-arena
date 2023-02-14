using System;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Game
{
    public enum PlayerID
    {
        Player1,
        Player2,
        Player3,
        Player4,
    }
    
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameSettingsSO _gameSettings;
        [SerializeField] private PlayerIDPlayerPair[] _playerEntries;
        [SerializeField] private PlayerSpawnPoint[] _playerSpawnPoint;
        [SerializeField] private int _playerDefaultLife;
        private Transform _parent;
        private Dictionary<PlayerID, float> _scoreboard;
        private Dictionary<PlayerID, int> _remainingLife;
        private Dictionary<PlayerID, PlayerStat> _playerList;

        private void Awake()
        {
            Assert.IsTrue(_playerEntries != null);
            Assert.IsTrue(_playerEntries.Length == GameSettingsSO.MAX_PLAYER_COUNT);
            Assert.IsTrue(_playerSpawnPoint != null);
            Assert.IsTrue(_playerSpawnPoint.Length == GameSettingsSO.MAX_PLAYER_COUNT);
        }

        public void Initialize()
        {
            _scoreboard = new Dictionary<PlayerID, float>();
            _remainingLife = new Dictionary<PlayerID, int>();
            _playerList = new Dictionary<PlayerID, PlayerStat>();
            _parent = new GameObject("Player Pool").GetComponent<Transform>();
            
            // TODO: better initialization required, current is not robust for inorder reference assignment
            // initialize all the values
            for (int i = 0; i < _gameSettings.PlayerCount; i++)
            {
                // initialize gameplay values
                _scoreboard.Add(_playerEntries[i].Id, 0);
                _remainingLife.Add(_playerEntries[i].Id, _playerDefaultLife);
                
                // spawn player instance
                PlayerStat playerStat = Instantiate(_playerEntries[i].PlayerStat, _parent);
                _playerList.Add(_playerEntries[i].Id, playerStat);
                // set up spawn point
                _playerSpawnPoint[i].Initialize(playerStat);
            }
        }

        /**
         * Instantiate the given player in the manager, return the Player Stat reference.
         */
        public PlayerStat SpawnPlayer(PlayerID id)
        {
            return Instantiate(_playerList[id], _parent);
        }

        /**
         * Increase the score of given player by given score.
         */
        public void IncreaseScore(PlayerID id, float score)
        {
            _scoreboard[id] += score;
            Debug.Log($"{id} got a score of {GetScore(id)}");
        }
        
        /**
         * Decrease the score of given player by given score.
         */
        public void DecreaseScore(PlayerID id, float score)
        {
            _scoreboard[id] -= score;
            Debug.Log($"{id} got a score of {GetScore(id)}");
        }

        /**
         * Deduct the remaining life of the given player by 1.
         */
        public void ReduceRemainingLife(PlayerID id)
        {
            _remainingLife[id]--;
            Debug.Log($"{id} got {GetRemainingLife(id)} life(s) left");
        }
        
        /**
         * Increase the remaining life of the given player by 1.
         */
        public void IncreaseRemainingLife(PlayerID id)
        {
            _remainingLife[id]++;
            Debug.Log($"{id} got {GetRemainingLife(id)} life(s) left");
        }

        public float GetScore(PlayerID id)
        {
            return _scoreboard[id];
        }

        public int GetRemainingLife(PlayerID id)
        {
            return _remainingLife[id];
        }

        public PlayerStat GetPlayerStat(PlayerID id) => _playerList[id];

        [Serializable]
        public struct PlayerIDPlayerPair
        {
            public PlayerID Id;
            public PlayerStat PlayerStat;
        }
    }
}