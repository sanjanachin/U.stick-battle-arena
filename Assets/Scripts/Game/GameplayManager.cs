using UnityEngine;

namespace Game
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private ProjectileManager _projectileManager;

        private void Awake()
        {
            _service.ProvideProjectileManager(_projectileManager);
        }
    }
}