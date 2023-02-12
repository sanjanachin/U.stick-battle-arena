using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public enum SceneID
    {
        MainMenu,
        Map1,
        Map2,
    }
    
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private GameService _gameService;
        [SerializeField] private SceneEntry[] _sceneEntries;
        private Dictionary<SceneID, string> _sceneMap;

        private void Awake()
        {
            _sceneMap = _sceneEntries.ToDictionary(
                    (entry) => entry.ID, // key
                    (entry) => AssetDatabase.GetAssetPath(entry.SceneAsset) // value
            );

            _gameService.ProvideSceneManager(this);
        }

        public void StartLoadScene(SceneID id)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneMap[id]);
            // TODO: add optional black screen fade in/out
            // TODO: add optional loading screen
        }

        [Serializable]
        private struct SceneEntry
        {
            public SceneID ID { get => _id; }
            public SceneAsset SceneAsset { get => _sceneAsset; }
            [SerializeField] private SceneID _id;
            [SerializeField] private SceneAsset _sceneAsset;
        }
    }
}