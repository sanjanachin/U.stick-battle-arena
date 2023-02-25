using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Game
{
    public class InitializeLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference _gameService;
        [SerializeField] private AssetReference _globalManagerScene;
        [SerializeField] private SceneID _firstSceneToLoad;

        private void Awake()
        {
            // load the first scene
            _globalManagerScene.LoadSceneAsync(
                LoadSceneMode.Additive, true).Completed += LoadGameService;
        }

        private void LoadGameService(AsyncOperationHandle<SceneInstance> handle)
        {
            _gameService.LoadAssetAsync<GameService>().Completed += LoadFirstScene;
        }

        private void LoadFirstScene(AsyncOperationHandle<GameService> handle)
        {
            Assert.IsTrue(handle.Result.SceneManager != null);
            handle.Result.SceneManager.LoadScene(_firstSceneToLoad);

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
        }
    }
}