using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private UsableItemID _usableItemID;
        [SerializeField] private float _spawnLifespan;
        
        [Header("Spawner Settings")]
        [SerializeField] private float _spawnInterval;
        [SerializeField] private float _randomXPosLowerBound;
        [SerializeField] private float _randomXPosUpperBound;
        private Vector3 _originalPosition;
        private float _currentTime;

        private void Awake()
        {
            _originalPosition = transform.position;
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (_currentTime > 0)
            {
                // update current count down time
                _currentTime -= Time.deltaTime;
            }
            else
            {
                // update the position of the spawner and spawn weapon
                SpawnWeapon();
                UpdatePosition();
                _currentTime = _spawnInterval;
            }
        }

        private void UpdatePosition()
        {
            // random x position
            transform.position = _originalPosition + new Vector3(
                Random.Range(_randomXPosLowerBound, _randomXPosUpperBound),
                transform.position.y);
        }

        private void SpawnWeapon()
        {
            // Get a weapon from the pool and set to the current location
            UsableItem weapon = _service.UsableItemManager.SpawnProjectile(_usableItemID);
            weapon.transform.position = transform.position;
            
            // Set the lifespan of this weapon
            weapon.Spawn(_spawnLifespan);
        }
    }
}
