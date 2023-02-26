#region

using Game.UI;
using TMPro;
using UnityEngine;

#endregion

namespace Game
{
    public class GameplayUIManager : MonoBehaviour
    {
        [SerializeField] private UI_PlayerStatusPanel _playerStatusPanel;
        // temporary winning screen
        // TODO: needs a designed winning effect
        [SerializeField] private TMP_Text _winningText;

        /**
         * Initialize gameplay UI elements,
         * should ONLY be called by GameplayManager
         */
        public void Initialize()
        {
            _playerStatusPanel.Initialize();
        }

        /**
         * Show the winning text for the player to screen
         * Temporary function, should be modified once the winning effect is decided 
         */
        public void ShowWinningScreen(PlayerID id)
        {
            _winningText.text = $"{id} WIN";
        }
    }
}