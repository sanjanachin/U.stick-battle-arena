using System;
using System.Collections;
using System.Collections.Generic;
using Game.DataSet;
using UnityEngine;

namespace Game
{
    public enum UsableItemID
    {
        Pistol = 1,
        Dagger = 2,
        Bow = 3,
        Sniper = 4,
        SMG = 5,
        Shotgun = 6,
        HandGrenade = 7,
        BaseballBat = 8,
        Spear = 9,
        BrassKnuckles = 10,
        Longsword = 11,
        Brick = 12,
    }
    
    public class UsableItemManager : MonoBehaviour
    {
        public static readonly UsableItemID[] UsableItemIDs =
        {
            UsableItemID.Bow, UsableItemID.Dagger, UsableItemID.Pistol, 
            UsableItemID.Shotgun, UsableItemID.Sniper, UsableItemID.HandGrenade, UsableItemID.SMG,
            UsableItemID.BaseballBat, UsableItemID.Spear, UsableItemID.BrassKnuckles, UsableItemID.Brick,
            UsableItemID.Longsword
        };
        
        [SerializeField] private UsableItemDataSetSO _usableItemData;

        private Dictionary<UsableItemID, GameObjectPool<UsableItem>> _poolmap;

        private void Awake()
        {
            _poolmap = new Dictionary<UsableItemID, GameObjectPool<UsableItem>>();

            foreach (UsableItemID id in UsableItemIDs)
            {
                Transform spawnParent = new GameObject($"{id} Pool").GetComponent<Transform>();
                spawnParent.SetParent(transform);

                _poolmap.Add(id, new GameObjectPool<UsableItem>(_usableItemData[id], spawnParent));
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
            usableItem.SetAndMoveToParent(_poolmap[id].PoolParent);
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
