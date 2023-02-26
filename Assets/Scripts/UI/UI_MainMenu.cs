#region

using UnityEngine;

#endregion

namespace Game
{
    public class UI_MainMenu : MonoBehaviour
    {
        [SerializeField] private GameService _gameService;
        [SerializeField] private SceneID _playGameButtonScene;
        
        public void PlayGame() 
        {
            _gameService.SceneManager.LoadScene(_playGameButtonScene);
        }

        public void QuitGame()
        {
            _gameService.SceneManager.ExitGame();
        }
    }
}