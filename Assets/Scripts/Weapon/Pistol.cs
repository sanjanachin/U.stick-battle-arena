using Game.Player;
using UnityEngine;

namespace Game
{
    public class Pistol : RangedWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Launch;
        }
    }
}