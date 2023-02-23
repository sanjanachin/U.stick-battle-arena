using UnityEngine;

namespace Game
{
    public class Brick : MeleeWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Attack;
        }

    }
}