#region

using UnityEngine;

#endregion

namespace Game
{
    /**
     * Provides references and common functions to any gameObjects
     * Should ONLY be referenced by gameObjects in Gameplay scenes
     */
    // Commented for the singleton property of the class
    // [CreateAssetMenu(menuName = "Game/GameplayService", fileName = "GameplayService")]
    public class GameplayService : ScriptableObject
    {
        public ProjectileManager ProjectileManager { get; private set; }
        public UsableItemManager UsableItemManager { get; private set; }
        public PlayerManager PlayerManager { get; private set; }
        public GameplayUIManager GameplayUIManager { get; private set; }

        public AudioManager AudioManager { get => _gameService.AudioManager; }
        [SerializeField] private GameService _gameService;

        /**
         * Any provide function below are to setup the reference of
         * managers of the gameplay scene.
         * Should ONLY be called by the GameplayManager of the scene.
         */
        public void ProvideProjectileManager(ProjectileManager projectileManager)
        {
            ProjectileManager = projectileManager;
        }

        public void ProvideUsableItemManager(UsableItemManager usableItemManager)
        {
            UsableItemManager = usableItemManager;
        }

        public void ProvidePlayerManager(PlayerManager playerManager)
        {
            PlayerManager = playerManager;
        }
        
        public void ProvideGameplayUIManager(GameplayUIManager gameplayUIManager)
        {
            GameplayUIManager = gameplayUIManager;
        }
    }
}