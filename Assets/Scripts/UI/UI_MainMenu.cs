using UnityEngine;

namespace Game
{
    public class UI_MainMenu : MonoBehaviour
    {
        [SerializeField] private GameService _gameService;
        [SerializeField] private SceneID _playGameButtonScene;
        
        public void PlayGame() 
        {
            _gameService.SceneManager.StartLoadScene(_playGameButtonScene);
        }

        public void QuitGame() 
        {
            Application.Quit();
        }
    }
}