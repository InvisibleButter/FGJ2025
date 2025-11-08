using System.Collections.Generic;
using Scripts.Grid;
using UnityEngine;

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
        private List<WatcherShroomEntity> _watchers;
        
        public void Initialize()
        {
            _watchers = new List<WatcherShroomEntity>();
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
                    var walkerAbility = new WalkerAbility(movementController.CurrentHittedEntity.GetCoordinate(), movementController.CameraForward, movementController);
                    walkerAbility.Execute();
                    break;
                case ShroomAbilityType.Watcher:
                    var watcherAbility = new WatcherAbility(movementController.CurrentHittedEntity.GetCoordinate(), movementController.CameraForward);
                    watcherAbility.Execute();
                    break;
            }
        }

        public void AddWatcher(Vector2 gridIndex, Vector3 rotation)
        {
            var watcher = ServiceLocator.Instance.GetService<ShroomGridService>().AddWatcherShroom(gridIndex, rotation);
            watcher.RefreshView();
            _watchers.Add(watcher);
        }

        public bool IsInitialized { get; set; }
    }
}