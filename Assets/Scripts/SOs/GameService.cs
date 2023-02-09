using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class GameService : ScriptableObject
    {
        [SerializeField] private AudioManager _audioManager;
    }
}