using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Player
{
    /**
     * Represent the inventory of a player
     */
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private GameplayService _service;
        [SerializeField] private Transform _itemHolderTrans;
        [SerializeField] private UsableItem _equippedItem;
        [SerializeField] private UsableItem _holdItem;
        [SerializeField] private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            _playerController.OnMovement += CheckAndFlip;
            _playerController.OnSwitchItem += SwitchItem;
        }
        
        // flip the item holder when the player is facing different directions
        private void CheckAndFlip()
        {
            _itemHolderTrans.localScale = new Vector3(
                _playerController.FacingLeft ? 1 : -1, 1, 1);
        }

        private void SwitchItem()
        {
            // do nothing if don't have another item
            if (_holdItem == null) return;
            
            // switch
            _equippedItem.UnEquip();
            _holdItem.Equip();
            (_equippedItem, _holdItem) = (_holdItem, _equippedItem);
        }

        /**
         * Set the item to the player's item holder
         * and updates player's inventory
         */
        public void PickUpItem(UsableItem item)
        {
            // check if currently holding an Item
            if (_equippedItem != null)
            {
                if (_holdItem == null)
                {
                    // holding one item, but got an empty slot
                    // set equipped item to holding equip pick up item 
                    _holdItem = _equippedItem;
                    _holdItem.UnEquip();
                }
                else
                {
                    // no empty slot, replace the item with equipped one
                    _equippedItem.UnEquip();
                    _equippedItem.ReturnToPool();
                }
            }
            
            _equippedItem = item;

            // move the gameObject under the player's holder
            item.SetAndMoveToParent(_itemHolderTrans);
        }
    }
}