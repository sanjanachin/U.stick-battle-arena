using Game.Player;
using UnityEngine;

namespace Game
{
    public class Sniper : RangedWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Launch;
        }
    }
}