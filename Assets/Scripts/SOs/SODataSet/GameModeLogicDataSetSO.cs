using UnityEngine;

namespace Game.DataSet
{
    public enum GameModeID
    {
        HighestScore,
        BattleRoyal,
        HighestKills,
    }
    
    [CreateAssetMenu]
    public class GameModeLogicDataSetSO : DataSetSO<GameModeID, GameModeLogicSO> { }
    
}