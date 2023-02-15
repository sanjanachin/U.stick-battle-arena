using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UI_MapSelection : MonoBehaviour
    {
        [SerializeField] private GameService _gameService;
        [SerializeField] private GameSettingsSO _gameSettings;
        
        public void LoadMap1()
        {
            _gameSettings.SetGameplayStageID(SceneID.Map1);
            _gameService.SceneManager.LoadScene(_gameSettings.GameplayStageID);
        }
        
        public void LoadMap2()
        {
            _gameSettings.SetGameplayStageID(SceneID.Map2);
            _gameService.SceneManager.LoadScene(_gameSettings.GameplayStageID);
        }
    }
}