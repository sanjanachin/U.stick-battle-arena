using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.UI
{
    public class UI_PlayerStatusPanel : MonoBehaviour
    {
        [SerializeField] private UI_PlayerStatusDisplay[] _playerStatusDisplays;

        private void Awake()
        {
            Assert.IsTrue(_playerStatusDisplays != null);
            Assert.IsTrue(_playerStatusDisplays.Length == GameSettingsSO.MAX_PLAYER_COUNT);
        }

        /**
         * Initialize the sub UIs
         * Should ONLY be called by the GameplayUIManager
         */
        public void Initialize()
        {
            foreach (UI_PlayerStatusDisplay display in _playerStatusDisplays)
                display.Initialize();
        }
    }
}