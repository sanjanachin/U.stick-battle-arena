using UnityEngine;

namespace Game
{
    public class Longsword : MeleeWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Attack;
        }

    }
}