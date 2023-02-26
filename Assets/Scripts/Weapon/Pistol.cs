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