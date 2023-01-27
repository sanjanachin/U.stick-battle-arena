using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Pickable : MonoBehaviour
    {
        public event UnityAction<PlayerController> OnPicked = delegate {  };
        
        // Invoke the OnPicked event on the player
        public void PickUpBy(PlayerController player)
        {
            if (player == null) return;
            OnPicked.Invoke(player);
        }
    }
}