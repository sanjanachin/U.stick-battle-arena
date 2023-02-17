using Game.Player;
using UnityEngine;

namespace Game
{
    public class GrenadeItem : RangedWeapon
    {
        private void Awake()
        {
            OnItemUseDown += Launch;
        }
    }
}