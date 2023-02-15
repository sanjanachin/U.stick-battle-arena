using UnityEngine;

namespace Game
{
    /**
     * Game mode logic for battle royal mode.
     * The game end with there's only one player with remaining lives
     * more than 0.
     */
    [CreateAssetMenu]
    public class BattleRoyalGameModeLogicSO : GameModeLogicSO
    {
        [SerializeField] private GameSettingsSO _gameSettings;
        [SerializeField] private GameplayService _gameplayService;

        protected override void HookEvents()
        {
            _gameplayService.PlayerManager.OnPlayerDies += CheckForWinner;
        }
        
        protected override void UnHookEvents()
        {
            _gameplayService.PlayerManager.OnPlayerDies -= CheckForWinner;
        }

        private void CheckForWinner()
        {
            int outCount = 0;
            PlayerID winnerId = PlayerID.Player1;
            // get the lives left for each player
            foreach (PlayerID id in GameSettingsSO.PLAYER_IDS)
            {
                if (_gameplayService.PlayerManager.GetRemainingLife(id) == 0)
                {
                    outCount++;
                }
                else
                {
                    winnerId = id;
                }
            }
            
            // if there are 3 zeros and one non zero the non zero player wins
            if (outCount == GameSettingsSO.MAX_PLAYER_COUNT - 1)
                InvokeGameEndedEvent(winnerId);
        }
    }
}