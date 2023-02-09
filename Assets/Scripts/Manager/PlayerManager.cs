using System;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;

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
        [SerializeField] private PlayerIDPlayerPair[] _playerEntries;
        private Transform _parent;
        private Dictionary<PlayerID, float> _scoreboard;
        private Dictionary<PlayerID, int> _remainingLife;
        private Dictionary<PlayerID, PlayerStat> _playerList;

        private void Awake()
        {
            _scoreboard = new Dictionary<PlayerID, float>();
            _remainingLife = new Dictionary<PlayerID, int>();
            _playerList = new Dictionary<PlayerID, PlayerStat>();
            _parent = new GameObject("Player Pool").GetComponent<Transform>();
            
            // initialize all the values
            for (int i = 0; i < _playerEntries.Length; i++)
            {
                _scoreboard.Add(_playerEntries[i].Id, 0);
                _remainingLife.Add(_playerEntries[i].Id, 10);
                _playerList.Add(_playerEntries[i].Id, _playerEntries[i].Player);
            }
        }

        public PlayerStat SpawnPlayer(PlayerID id)
        {
            return Instantiate(_playerList[id], _parent);
        }

        public void IncreaseScore(PlayerID id, float score)
        {
            _scoreboard[id] += score;
            Debug.Log($"{id} got a score of {GetScore(id)}");
        }
        
        public void DecreaseScore(PlayerID id, float score)
        {
            _scoreboard[id] -= score;
            Debug.Log($"{id} got a score of {GetScore(id)}");
        }

        public void ReduceRemainingLife(PlayerID id)
        {
            _remainingLife[id]--;
            Debug.Log($"{id} got {GetRemainingLife(id)} life(s) left");
        }
        
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

        [Serializable]
        public struct PlayerIDPlayerPair
        {
            public PlayerID Id;
            public PlayerStat Player;
        }
    }
}
