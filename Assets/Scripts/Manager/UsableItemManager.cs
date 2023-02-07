using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum UsableItemID
    {
        Pistol,
        Dagger,
        Bow,
    }
    public class UsableItemManager : MonoBehaviour
    {
        [SerializeField] private UsableItemIdPrefabPair[] _prefabEntries;

        private Dictionary<UsableItemID, GameObjectPool<UsableItem>> _poolmap;

        private void Awake()
        {
            _poolmap = new Dictionary<UsableItemID, GameObjectPool<UsableItem>>();

            for (int i = 0; i < _prefabEntries.Length; i++)
            {
                Transform spawnParent = new GameObject($"{_prefabEntries[i].Id} Pool").GetComponent<Transform>();
                
                _poolmap.Add(_prefabEntries[i].Id,
                    new GameObjectPool<UsableItem>(_prefabEntries[i].UsableItem, spawnParent));
            }
        }
        
        /**
         * Get a usable item prefab from the pool and return it
         */
        public UsableItem SpawnProjectile(UsableItemID id)
        {
            GameObjectPool<UsableItem> pool = _poolmap[id];
            UsableItem usableItem = pool.Get(_ => { });
            return usableItem;
        }
        
        /**
         * Return the usable item gameObject to the pool
         */
        public void ReturnUsableItem(UsableItemID id, UsableItem usableItem)
        {
            _poolmap[id].Release(usableItem);
        }

        [Serializable]
        private struct UsableItemIdPrefabPair
        {
            public UsableItemID Id;
            public UsableItem UsableItem;
        }
    }
}
