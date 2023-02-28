using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public enum SceneID
    {
        // GlobalManagers = 1,
        MainMenu = 2,
        MapSelectionMenu = 3,
        Map1 = 10,
        Map2 = 11,
    }
    
    /**
     * Respond for loading and unloading scenes
     */
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private GameService _gameService;
        [SerializeField] private SceneID _currentScene;

        private void Awake()
        {
            _gameService.ProvideSceneManager(this);
        }

        /**
         * Unload current scene and load the new scene
         */
        public void LoadScene(SceneID id)
        {
            UnityEngine.SceneManagement.SceneManager
                .UnloadSceneAsync(_currentScene.ToString());
            AdditionLoadScene(id);
        }

        /**
         * Load the first scene and unload the default scene
         */
        public void AdditionLoadScene(SceneID id)
        {
            _currentScene = id;
            UnityEngine.SceneManagement.SceneManager
                .LoadScene(id.ToString(), LoadSceneMode.Additive);
        }

        public void ExitGame()
        {
            App.ExitApplication();
        }
    }
}