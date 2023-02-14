using Game.UI;
using UnityEngine;

namespace Game
{
    public class GameplayUIManager : MonoBehaviour
    {
        [SerializeField] private UI_PlayerStatusPanel _playerStatusPanel;

        public void Initialize()
        {
            _playerStatusPanel.Initialize();
        }
    }
}