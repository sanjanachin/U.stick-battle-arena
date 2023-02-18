using UnityEngine;

namespace Game
{
    public class Spear : MeleeWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Attack;
        }
    }
}