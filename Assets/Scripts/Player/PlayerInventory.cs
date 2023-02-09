using System.Collections;
using UnityEngine;
using Object = System.Object;

namespace Game.Player
{
    /**
     * Represent the inventory of a player
     */
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Transform _itemHolderTrans;
        [SerializeField] private UsableItem _equippedItem;
        [SerializeField] private UsableItem _holdItem;
        [SerializeField] private PlayerController _playerController;
        [Tooltip("The delay applied before equipping the remaining item if the current item broke")]
        [SerializeField] private float _itemBreakSwitchDelay;
        
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

        // delay a bit then check and equip the current holding item
        private IEnumerator ReEquipItemAfterBreak()
        {
            // TODO: very ugly by-pass, might need to modify for edge cases
            yield return new WaitForSecondsRealtime(_itemBreakSwitchDelay);
            if (!Object.Equals(_equippedItem, null) && !_equippedItem.isActiveAndEnabled)
                _equippedItem.Equip();
        }

        // handle player's switch action, switch items in inventory
        private void SwitchItem()
        {
            // do nothing if don't have another item
            if (Object.Equals(_holdItem, null)) return;
            
            // switch
            _equippedItem.UnEquip();
            _holdItem.Equip();
            (_equippedItem, _holdItem) = (_holdItem, _equippedItem);
        }

        // handle if the item is broken
        // automatically equip the remaining item
        private void HandleItemBreak(UsableItem item)
        {
            // place the broken item to hold slot
            if (item == _equippedItem)
                (_equippedItem, _holdItem) = (_holdItem, _equippedItem);
            
            // disable item
            _holdItem.UnEquip();
            _holdItem.ReturnToPool();
            _holdItem = null;

            // delay a bit before equip the hold item to prevent double shooting
            StartCoroutine(ReEquipItemAfterBreak());
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
            item.OnBreak += HandleItemBreak;

            // move the gameObject under the player's holder
            item.SetAndMoveToParent(_itemHolderTrans);
        }
    }
}