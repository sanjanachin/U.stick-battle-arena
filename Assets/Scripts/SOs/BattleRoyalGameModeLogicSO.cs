using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class BattleRoyalGameModeLogicSO : GameModeLogicSO
    {
        [SerializeField] private GameSettingsSO _gameSettings;
        [SerializeField] private GameplayService _gameplayService;

        protected override void InitializeLogic()
        {
            _gameplayService.PlayerManager.OnPlayerDies += CheckForWinner;
        }

        private void CheckForWinner()
        {
            int outCount = 0;
            foreach (PlayerID id in GameSettingsSO.PLAYER_IDS)
            {
                if (_gameplayService.PlayerManager.GetRemainingLife(id) == 0)
                    outCount++;
            }
            
            if (outCount == GameSettingsSO.MAX_PLAYER_COUNT - 1)
                InvokeGameEndedEvent();
        }
    }
}