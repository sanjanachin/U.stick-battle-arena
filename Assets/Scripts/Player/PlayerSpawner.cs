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
        private PlayerController _player;

        private void Start()
        {
            _player = _service.PlayerManager.SpawnPlayer(_playerID);
            _player.transform.position = transform.position;
        }
    }
}
