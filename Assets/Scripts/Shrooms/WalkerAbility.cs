using Scripts.Grid;
using UnityEngine;

namespace Scripts.Shrooms
{
    public class WalkerAbility : IShroomAbility
    {
        private readonly ShroomGridService _gridService;
        private readonly Vector2 _startCoordinate;
        
        public WalkerAbility(Vector2 startCoordinate)
        {
            _startCoordinate = startCoordinate;
            _gridService = ServiceLocator.Instance.GetService<ShroomGridService>();
        }
        
        public void Execute()
        {
            var toAdd = _gridService.GetAdjacentTilesOfState(_startCoordinate, GridState.Unlocked);

            foreach (var tile in toAdd)
            {
                tile.ChangeGridState(GridState.Occupied);
            }
        }
    }
}