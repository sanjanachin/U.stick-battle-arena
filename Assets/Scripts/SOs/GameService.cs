#region

using UnityEngine;

#endregion

namespace Game
{
    [CreateAssetMenu]
    public class GameService : ScriptableObject
    {
        public AudioManager AudioManager { get; private set; }
        public SceneManager SceneManager { get; private set; }

        public void ProvideAudioManager(AudioManager _audioManager)
        {
            AudioManager = _audioManager;
        }
        
        public void ProvideSceneManager(SceneManager _sceneManager)
        {
            SceneManager = _sceneManager;
        }
    }
}