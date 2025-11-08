
using UnityEngine;

namespace Scripts.Shrooms
{
    public class WatcherAbility : IShroomAbility
    {
        private Vector2 _startCoordinate;
        private float _watcherAngle = 45f;
        
        public WatcherAbility (Vector2 startCoordinate)
        {
            _startCoordinate = startCoordinate;
        }
        
        public void Execute()
        {
            ServiceLocator.Instance.GetService<ShroomAbilityService>().AddWatcher(_startCoordinate);
        }
    }
}