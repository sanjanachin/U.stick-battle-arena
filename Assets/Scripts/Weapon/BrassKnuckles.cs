using UnityEngine;

namespace Game
{
    public class BrassKnuckles : MeleeWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Attack;
        }

    }
}