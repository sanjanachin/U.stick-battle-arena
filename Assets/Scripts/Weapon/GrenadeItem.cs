using Game.Player;
using UnityEngine;

namespace Game
{
    public class GrenadeItem : RangedWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Launch;
        }
    }
}