using UnityEngine;

namespace Game.Player
{
    /**
     * Represent the inventory of a player
     */
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Transform _itemHolderTrans;
        [SerializeField] private UsableItem _currentItem;
        [SerializeField] private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            _playerController.OnMovement += CheckAndFlip;
        }
        
        // flip the item holder when the player is facing different directions
        private void CheckAndFlip()
        {
            _itemHolderTrans.localScale = new Vector3(
                _playerController.FacingLeft ? 1 : -1, 1, 1);
        }
        
        /**
         * Set the item to the player's item holder
         * and updates player's inventory
         */
        public void PickUpItem(UsableItem item)
        {
            _currentItem = item;
            
            // move the gameObject under the player's holder
            item.transform.SetParent(_itemHolderTrans, false);
            item.transform.localPosition = Vector3.zero;
        }
    }
}