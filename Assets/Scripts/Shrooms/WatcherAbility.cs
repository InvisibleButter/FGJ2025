
using UnityEngine;

namespace Scripts.Shrooms
{
    public class WatcherAbility : IShroomAbility
    {
        private Vector2 _startCoordinate;
        private Vector3 _watchRotation;
        private float _watcherAngle = 45f;
        
        public WatcherAbility (Vector2 startCoordinate, Vector3 watcherDirection)
        {
            _startCoordinate = startCoordinate;
            _watchRotation = watcherDirection;
        }
        
        public void Execute()
        {
            ServiceLocator.Instance.GetService<ShroomAbilityService>().AddWatcher(_startCoordinate, _watchRotation);
        }
    }
}