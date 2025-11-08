using Scripts.Shrooms;

namespace Scripts
{
    public class MyGameManager : GameManager
    {
        public MovementController MovementController;
        
        protected override void AddAdditionalServices()
        {
            base.AddAdditionalServices();
            ServiceLocator.Instance.Register<ShroomAbilityService>(new  ShroomAbilityService());
        }
    }
}