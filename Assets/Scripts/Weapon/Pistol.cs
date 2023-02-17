using Game.Player;
using UnityEngine;

namespace Game
{
    public class Pistol : RangedWeapon
    {
        private void Start()
        {
            OnItemUseDown += Launch;
        }
    }
}