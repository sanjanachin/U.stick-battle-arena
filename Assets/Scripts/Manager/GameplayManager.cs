using Game.DataSet;
using UnityEngine;

namespace Game
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private GameSettingsSO _gameSettings;
        [SerializeField] private GameService _gameService;
        [SerializeField] private GameplayService _service;
        [SerializeField] private GameModeLogicDataSetSO _gameModeLogicDataSet;
        
        [Header("Manager References")]
        [SerializeField] private ProjectileManager _projectileManager;
        [SerializeField] private UsableItemManager _usableItemManager;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private GameplayUIManager _gameplayUIManager;

        [SerializeField] private GameModeLogicSO _gameModeLogic;
        
        private void Awake()
        {
            // temporary set game mode for battle royal mode
            // TODO: wait for game mode menu and change this
            _gameSettings.SetGameMode(GameModeID.BattleRoyal);
            _gameModeLogic = _gameModeLogicDataSet[_gameSettings.GameModeID];
            
            _service.ProvideProjectileManager(_projectileManager);
            _service.ProvideUsableItemManager(_usableItemManager);
            _service.ProvidePlayerManager(_playerManager);
            _service.ProvideGameplayUIManager(_gameplayUIManager);
            
            // Initialize managers in order of dependency
            _playerManager.Initialize();
            _gameplayUIManager.Initialize();
            
            // initialize and hook the game ended event
            _gameModeLogic.Initialize();
            _gameModeLogic.OnGameEnded += HandleGameEnded;
        }

        private void HandleGameEnded(PlayerID winnerId)
        {
            Debug.Log("Game ended");
            // temporary winning effect
            // slow time and wait for 4 seconds to load back to main menu
            Time.timeScale = 0.25f;
            _service.GameplayUIManager.ShowWinningScreen(winnerId);
            Invoke(nameof(LoadBackToMainMenu), 1f);
        }

        private void LoadBackToMainMenu()
        {
            Time.timeScale = 1f;
            _gameService.SceneManager.LoadScene(SceneID.MainMenu);
        }
    }
}