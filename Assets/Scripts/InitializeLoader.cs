using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class InitializeLoader : MonoBehaviour
    {
        [SerializeField] private GameService _gameService;
        [SerializeField] private SceneID _firstSceneToLoad;

        private void Awake()
        {
            // load the first scene
            _gameService.SceneManager.FirstLoadScene(_firstSceneToLoad);
        }
    }
}