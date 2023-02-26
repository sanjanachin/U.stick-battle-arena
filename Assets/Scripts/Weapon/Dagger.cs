namespace Game
{
    public class Dagger : MeleeWeapon
    {
        protected override void Initialize()
        {
            OnItemUseDown += Attack;
        }
    }
}