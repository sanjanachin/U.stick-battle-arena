using System;
using System.Collections;
using System.Collections.Generic;
using Game.Player;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UI_PlayerStatus : MonoBehaviour
    {
        [SerializeField] private GameSettingsSO _gameSettings;
        [SerializeField] private GameplayService _service;
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private TMP_Text _lifeLeftLabel;
        [SerializeField] private PlayerID _playerID;

        private void Awake()
        {
            // deactivate player status display if the player is not in the gameplay
            if (!_gameSettings.PlayerIDInGameplay(_playerID))
            {
                gameObject.SetActive(false);
                return;
            }
            
            // set up event hooks
            PlayerStat _playerStat = _service.PlayerManager.GetPlayerStat(_playerID);
            // _playerStat.OnHealthChange += HandleHealthChange;
        }

        private void HandleHealthChange(int value)
        {
            
        }
    }
}