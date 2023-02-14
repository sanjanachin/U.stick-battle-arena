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

        public void Initialize()
        {
            foreach (UI_PlayerStatusDisplay display in _playerStatusDisplays)
                display.Initialize();
        }
    }
}