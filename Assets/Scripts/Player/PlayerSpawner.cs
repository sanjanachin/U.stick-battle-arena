using System;
using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private PlayerID _playerID;
        private PlayerStat _player;

        private void Start()
        {
            _player = _service.PlayerManager.SpawnPlayer(_playerID);
            _player.transform.position = transform.position;
            _player.OnDeath += Respawn;
        }

        /**
         * Change the position of the player to the position of the spawner
         */
        private void Respawn()
        {
            _player.transform.position = transform.position;
        }
    }
}
