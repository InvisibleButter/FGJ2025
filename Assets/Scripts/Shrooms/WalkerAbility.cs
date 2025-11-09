using System;
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
        private Action _action;
        
        public WalkerAbility(MovementController movementController, Action onFinish)
        {
            _startCoordinate = movementController.CurrentHittedEntity.GetCoordinate();
            _forwardDirection = movementController.CameraForward;
            _movementController = movementController;
            _gridService = ServiceLocator.Instance.GetService<ShroomGridService>();
            _action = onFinish;
        }
        
        public void Execute()
        {
            var toAdd = _gridService.GetTilesHitByCharge(_startCoordinate, _forwardDirection, 5f);
            
            foreach (var tile in toAdd)
            {
                tile.ChangeGridState(GridState.Occupied, true);
            }

            var target = toAdd.Last();
            if (target.GridTileType != GridTileType.Wall)
            {
                _movementController.FlyToPoint(target.transform.position, _action);
            }
            else
            {
                if (toAdd.Count < 2)
                {
                    _movementController.MovementAllowed = true;
                    return;
                }
                target = toAdd[^2];
                _movementController.FlyToPoint(target.transform.position, _action);
            }
        }
    }
}