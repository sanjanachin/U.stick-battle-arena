using UnityEngine;

namespace Game.DataSet
{
    public enum GameModeID
    {
        HighestScore,
        BattleRoyal,
        HighestKills,
    }
    
    [CreateAssetMenu(menuName = "Game/DataSet/GameModeLogic")]
    public class GameModeLogicDataSetSO : DataSetSO<GameModeID, GameModeLogicSO> { }
    
}