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
        [SerializeField] private TMP_Text _lifeCountLabel;
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
            gameObject.SetActive(false);
            // deactivate player status display if the player is not in the gameplay
            if (!_gameSettings.PlayerIDInGameplay(_playerID)) return;
            gameObject.SetActive(true);

            // set up event hooks
            PlayerStat playerStat = _service.PlayerManager.GetPlayerStat(_playerID);
            playerStat.OnHealthChange += UpdateHealthBarVisual;
            playerStat.OnDeath += UpdateLifeCountVisual;

            PlayerInventory playerInventory = playerStat.GetComponent<PlayerInventory>();
            playerInventory.OnItemSwitched += UpdateInventoryIcon;
            playerInventory.OnItemEquip += HookToItemDurabilityChange;
            playerInventory.OnItemUnEquip += UnHookToItemDurabilityChange;
            playerInventory.OnItemPick += UpdateDurabilityBar;
            
            _service.PlayerManager.OnScoreChange += UpdateScore;
        }

        private void UpdateHealthBarVisual(PlayerStat playerStat)
        {
            _healthBar.fillAmount = playerStat.HealthPercentage * 0.5f;
        }

        private void HookToItemDurabilityChange(UsableItem equippedItem)
        {
            equippedItem.OnDurabilityChange += UpdateDurabilityBar;
            UpdateDurabilityBar(equippedItem);
        }

        private void UnHookToItemDurabilityChange(UsableItem unequippedItem)
        {
            unequippedItem.OnDurabilityChange -= UpdateDurabilityBar;
        }

        private void UpdateDurabilityBar(UsableItem item)
        {
            _itemDurabilityBar.fillAmount = item.DurabilityPercentage * 0.5f;
        }
        
        // TODO : rework on inventory icon, currently coded for demo
        private void UpdateInventoryIcon(UsableItem equippedItem, UsableItem holdItem)
        {
            if (holdItem == null)
            {
                _itemIcon.gameObject.SetActive(false);
                return;
            }
            
            _itemIcon.gameObject.SetActive(true);
            _itemIcon.sprite = holdItem.Icon;
        }
        
        private void UpdateLifeCountVisual(int lifeCount)
        {
            _lifeCountLabel.text = $"{lifeCount}";
        }

        private void UpdateScore(PlayerID id)
        {
            if (id != _playerID) return;
            _scoreLabel.text = $"{_service.PlayerManager.GetScore(id)}";
        }
    }
}