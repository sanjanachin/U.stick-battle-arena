using UnityEngine;

namespace Game
{
    public class Dagger : MeleeWeapon
    {
        private void Start()
        {
            OnItemUseDown += Attack;
        }
    }
}