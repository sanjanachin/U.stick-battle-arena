using Game.Player;
using UnityEngine;

namespace Game
{
    public class Sniper : RangedWeapon
    {
        private void Awake()
        {
            OnItemUseDown += Launch;
        }
    }
}