using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        public GameObject weapon;
        public float spawnTime;
        private float currentTime;

        void Update()
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (currentTime > 0)
            {
                // update current count down time
                currentTime -= Time.deltaTime;
            }
            else
            {
                // update the position of the spawner and spawn weapon
                UpdatePosition();
                SpawnWeapon();
                currentTime = spawnTime;
            }
        }

        private void UpdatePosition()
        {
            // random x position
            transform.position = new Vector3(Random.Range(-15, 15), transform.position.y);
        }

        private void SpawnWeapon()
        {
            Instantiate(weapon, transform.position, Quaternion.identity);
        }
    }
}
