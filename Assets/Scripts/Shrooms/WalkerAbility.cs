using System.Linq;
using Scripts.Grid;
using UnityEngine;

namespace Scripts.Shrooms
{
    public class WalkerAbility : IShroomAbility
    {
        private readonly ShroomGridService _gridService;
        private readonly Vector2 _startCoordinate;
        private readonly Vector3 _forwardDirection;
        private readonly MovementController _movementController;
        
        public WalkerAbility(Vector2 startCoordinate, Vector3 forwardDirection, MovementController movementController)
        {
            _startCoordinate = startCoordinate;
            _forwardDirection = forwardDirection;
            _movementController = movementController;
            _gridService = ServiceLocator.Instance.GetService<ShroomGridService>();
        }
        
        public void Execute()
        {
            var toAdd = _gridService.GetTilesHitByCharge(_startCoordinate, _forwardDirection, 5f);
            
            foreach (var tile in toAdd)
            {
                tile.ChangeGridState(GridState.Occupied);
            }

            var target = toAdd.Last();
            if (target.GridTileType != GridTileType.Wall)
            {
                _movementController.FlyToPoint(target.transform.position);
            }
            else
            {
                if (toAdd.Count < 2)
                {
                    _movementController.MovementAllowed = true;
                    return;
                }
                target = toAdd[^2];
                _movementController.FlyToPoint(target.transform.position);
            }
        }
    }
}