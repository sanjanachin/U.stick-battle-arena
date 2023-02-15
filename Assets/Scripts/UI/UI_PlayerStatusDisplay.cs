using System;
using Game.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UI_PlayerStatusDisplay : MonoBehaviour
    {
        [SerializeField] private GameSettingsSO _gameSettings;
        [SerializeField] private GameplayService _service;

        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private TMP_Text _lifeLeftLabel;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _itemDurabilityBar;
        [SerializeField] private Image _itemIcon;

        [SerializeField] private PlayerID _playerID;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        /**
         * Initialize the UI by hooking to corresponded events
         * Should ONLY be called by the corresponding manager
         */
        public void Initialize()
        {
            // deactivate player status display if the player is not in the gameplay
            if (!_gameSettings.PlayerIDInGameplay(_playerID)) return;
            gameObject.SetActive(true);

            // set up event hooks
            PlayerStat playerStat = _service.PlayerManager.GetPlayerStat(_playerID);
            playerStat.OnHealthChange += UpdateHealthBarVisual;

            _service.PlayerManager.OnScoreChange += UpdateScore;

        }

        private void UpdateHealthBarVisual(int hp, int maxHp)
        {
            _healthBar.fillAmount = (float)hp * 0.5f / maxHp;
        }

        private void UpdateScore(PlayerID id)
        {
            _scoreLabel.text = _service.PlayerManager.GetScore(id).ToString();
        }
    }
}