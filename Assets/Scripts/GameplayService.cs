using UnityEngine;

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

        /**
         * Any provide function below are to setup the reference of
         * managers of the gameplay scene.
         * Should ONLY be called by the GameplayManager of the scene.
         */
        public void ProvideProjectileManager(ProjectileManager projectileManager)
        {
            ProjectileManager = projectileManager;
        }
    }
}