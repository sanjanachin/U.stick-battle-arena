using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class GameSettingsSO : ScriptableObject
    {
        public static readonly int MIN_PLAYER_COUNT = 2;
        public static readonly int MAX_PLAYER_COUNT = 4;

        public int PlayerCount { get; private set; } = MIN_PLAYER_COUNT;
        public SceneID GameplayStageID { get; private set; }

        /**
         * Check if the given player id is in the gameplay
         * return true if the given id is in the gameplay, false otherwise
         */
        public bool PlayerIDInGameplay(PlayerID id)
        {
            if (id == PlayerID.Player3 && PlayerCount == MIN_PLAYER_COUNT) return false;
            if (id == PlayerID.Player4 && PlayerCount != MAX_PLAYER_COUNT) return false;
            return true;
        }
        
        public void SetPlayerCount(int value)
        {
            if (value < MIN_PLAYER_COUNT || value > MAX_PLAYER_COUNT)
            {
                Debug.LogError($"Trying to set invalid player count: {value}!");
                return;
            }

            PlayerCount = value;
        }
        
        public void SetGameplayStageID(SceneID value)
        {
            GameplayStageID = value;
        }
    }
}