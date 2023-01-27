using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Transform _itemHolderTrans;
        [SerializeField] private GameObject _currentItem;
        [SerializeField] private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            _playerController.OnMovement += CheckAndFlip;
        }
        
        private void CheckAndFlip()
        {
            _itemHolderTrans.localScale = new Vector3(
                _playerController.FacingLeft ? 1 : -1, 1, 1);
        }
        
        // detect for collision
        private void OnCollisionEnter2D(Collision2D col)
        {
            // check if collided with a usable item
            UsableItem item = col.gameObject.GetComponent<UsableItem>();
            if (item == null) return;
            item.PickUpBy(_playerController);
            // move the gameobject under the player's holder and reset position
            item.transform.position = Vector3.zero;
            item.transform.SetParent(_itemHolderTrans, false);
        }
    }
}