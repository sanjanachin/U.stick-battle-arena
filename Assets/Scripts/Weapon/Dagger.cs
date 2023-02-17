using UnityEngine;

namespace Game
{
    public class Dagger : MeleeWeapon
    {
        private void Awake()
        {
            OnItemUseDown += Attack;
        }
    }
}