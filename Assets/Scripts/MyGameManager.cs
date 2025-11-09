using Scripts.Shrooms;
using UnityEngine;

namespace Scripts
{
    public enum GameState
    {
        ShroomSelection,
        Running,
        GameOver,
    }
    
    public class MyGameManager : GameManager
    {
        [SerializeField] private ShroomSelectionView shroomSelection;
        public GameState State { get; private set; }

        protected override void AddAdditionalServices()
        {
            base.AddAdditionalServices();
            ServiceLocator.Instance.Register<ShroomAbilityService>(new ShroomAbilityService());
        }

        protected override void OnAllInitialized()
        {
            base.OnAllInitialized();

            State = GameState.ShroomSelection;
            shroomSelection.Show();
        }
    }

}