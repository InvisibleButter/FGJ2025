namespace Scripts.Shrooms
{
    public enum ShroomAbilityType
    {
        Walker = 1,
        Watcher = 2,
        Builder = 3
    }
    
    public class ShroomAbilityService : IService
    {
        public void Initialize()
        {
            IsInitialized = true;
        }

        public void DeInitialize()
        {
            IsInitialized = false;
        }

        public void OnAbilityClicked(ShroomAbilityType selection, MovementController movementController)
        {
            switch (selection)
            {
                case ShroomAbilityType.Walker:
                    var walkerAbility = new WalkerAbility(movementController.CurrentHittedEntity.Coordinate);
                    walkerAbility.Execute();
                    break;
            }
        }

        public bool IsInitialized { get; set; }
    }
}