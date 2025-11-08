using Scripts.Grid;
using UnityEngine;

namespace Scripts.Shrooms
{
    public class WalkerAbility : IShroomAbility
    {
        private readonly ShroomGridService _gridService;
        private readonly Vector2 _startCoordinate;
        private readonly Vector3 _forwardDirection;
        
        public WalkerAbility(Vector2 startCoordinate, Vector3 forwardDirection)
        {
            _startCoordinate = startCoordinate;
            _forwardDirection = forwardDirection;
            _gridService = ServiceLocator.Instance.GetService<ShroomGridService>();
        }
        
        public void Execute()
        {
            var toAdd = _gridService.GetTilesHitByCharge(_startCoordinate, _forwardDirection, 5f);
            
            foreach (var tile in toAdd)
            {
                tile.ChangeGridState(GridState.Occupied);
            }
        }
    }
}