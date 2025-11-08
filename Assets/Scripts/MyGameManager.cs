using Scripts.Shrooms;

namespace Scripts
{
    public class MyGameManager : GameManager
    {
        protected override void AddAdditionalServices()
        {
            base.AddAdditionalServices();
            ServiceLocator.Instance.Register<ShroomAbilityService>(new  ShroomAbilityService());
        }
    }
}