using UnityEngine;

namespace Game
{
    public class BaseballBat : MeleeWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Attack;
        }
    }
}