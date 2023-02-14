using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game
{
    public enum SceneID
    {
        MainMenu,
        Map1,
        Map2,
    }
    
    /**
     * Respond for loading and unloading scenes
     */
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private GameService _gameService;
        [SerializeField] private SceneEntry[] _sceneEntries;
        [SerializeField] private SceneID _currentScene;
        private Dictionary<SceneID, AssetReference> _sceneMap;

        private void Awake()
        {
            _sceneMap = _sceneEntries.ToDictionary(
                    (entry) => entry.ID, // key
                    (entry) => entry.SceneReference // value
            );

            _gameService.ProvideSceneManager(this);
        }

        /**
         * Unload current scene and load the new scene
         */
        public void LoadScene(SceneID id)
        {
            _sceneMap[_currentScene].UnLoadScene();
            StartCoroutine(LoadingProcess(id));
        }

        /**
         * Load the first scene and unload the default scene
         */
        public void FirstLoadScene(SceneID id)
        {
            _currentScene = id;
            StartCoroutine(LoadingProcess(id));
        }
        
        private IEnumerator LoadingProcess(SceneID id)
        {
            // load scene in async
            AsyncOperationHandle<SceneInstance> loadingHandle = 
                _sceneMap[id].LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            
            // wait for loading
            while (!loadingHandle.IsDone)
                yield return null;
            
            // activate scene
            UnityEngine.SceneManagement.SceneManager
                .SetActiveScene(loadingHandle.Result.Scene);
            _currentScene = id;
        }
        
        public void ExitGame()
        {
            App.ExitApplication();
        }
        
        [Serializable]
        private struct SceneEntry
        {
            public SceneID ID { get => _id; }
            public AssetReference SceneReference { get => _sceneReference; }
            [SerializeField] private SceneID _id;
            [SerializeField] private AssetReference _sceneReference;
        }
    }
}