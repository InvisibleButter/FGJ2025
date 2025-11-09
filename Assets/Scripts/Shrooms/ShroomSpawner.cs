using Scripts.Grid;
using UnityEngine;

namespace Scripts.Shrooms
{
    public class ShroomSpawner : MonoBehaviour, IService
    {
        [SerializeField] private GameObject watcherShroomPrefab, walkerShroomPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private MyGameManager gameManager;
        
        public MovementController CurrentShroom => _currentShroom;

        public ShroomAbilityType CurrentShroomType => _currentShroomType;

        private MovementController _currentShroom;
        private ShroomAbilityType _currentShroomType;
        
        public void OnShroomSelected(ShroomAbilityType shroomType)
        {
            var prefab = shroomType == ShroomAbilityType.Walker ? walkerShroomPrefab : watcherShroomPrefab;
            _currentShroomType = shroomType;
            _currentShroom = Instantiate(prefab, spawnPoint).GetComponent<MovementController>();
            ServiceLocator.Instance.GetService<ShroomGridService>().UpDateAllCells(shroomType == ShroomAbilityType.Watcher);
            gameManager.ChangeGameState(GameState.Running);
        }

        public void DespawnShroom()
        {
            Destroy(_currentShroom.gameObject);
            _currentShroom = null;
            
            gameManager.ChangeGameState(GameState.ShroomSelection);
        }
        
        public void Initialize()
        {
            IsInitialized = true;
        }

        public void DeInitialize()
        {
            IsInitialized = false;
        }

        public bool IsInitialized { get; set; }
    }
}