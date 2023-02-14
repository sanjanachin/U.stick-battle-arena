using Game.DataSet;
using UnityEngine;
using UnityEngine.Events;

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
            _gameSettings.SetGameMode(GameModeID.BattleRoyal);
            _gameModeLogic = _gameModeLogicDataSet[_gameSettings.GameModeID];
            
            _service.ProvideProjectileManager(_projectileManager);
            _service.ProvideUsableItemManager(_usableItemManager);
            _service.ProvidePlayerManager(_playerManager);
            _service.ProvideGameplayUIManager(_gameplayUIManager);
            
            // Initialize managers in order of dependency
            _playerManager.Initialize();
            _gameplayUIManager.Initialize();

            _gameModeLogic.Initialize();
            _gameModeLogic.OnGameEnded += HandleGameEnded;
        }

        private void HandleGameEnded()
        {
            Debug.Log("Game ended");
            _gameService.SceneManager.LoadScene(SceneID.MainMenu);
        }
    }
}