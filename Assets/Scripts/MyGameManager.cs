using Scripts.Shrooms;
using Scripts.UI;
using UnityEngine;

namespace Scripts
{
    public enum GameState
    {
        None,
        ShroomSelection,
        Running,
        GameOver,
    }
    
    public class MyGameManager : GameManager
    {
        [SerializeField] private ShroomSelectionView shroomSelection;
        [SerializeField] private GameOverView gameOverView;
        
        public GameState State { get; private set; }

        protected override void AddAdditionalServices()
        {
            base.AddAdditionalServices();
            ServiceLocator.Instance.Register<ShroomAbilityService>(new ShroomAbilityService());
        }

        protected override void OnAllInitialized()
        {
            base.OnAllInitialized();

            ChangeGameState(GameState.ShroomSelection);
        }

        public void ChangeGameState(GameState newState)
        {
            State = newState;

            switch (State)
            {
                case GameState.ShroomSelection:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    gameOverView.Hide();
                    shroomSelection.Show();
                    break;
                case GameState.GameOver:
                    gameOverView.Show();
                    break;
            }
        }
    }

}