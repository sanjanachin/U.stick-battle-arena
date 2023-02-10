using UnityEngine;

namespace Game
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private ProjectileManager _projectileManager;
        [SerializeField] private UsableItemManager _usableItemManager;
        [SerializeField] private PlayerManager _playerManager;

        private void Awake()
        {
            _service.ProvideProjectileManager(_projectileManager);
            _service.ProvideUsableItemManager(_usableItemManager);
            _service.ProvidePlayerManager(_playerManager);
        }
    }
}