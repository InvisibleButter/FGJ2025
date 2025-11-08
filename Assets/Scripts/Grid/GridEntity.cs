using System;
using UnityEngine;

namespace Scripts.Grid
{
    public class GridEntity : MonoBehaviour
    {
        public GridTileType GridTileType;

        public Vector2 _coordinate;
        private GridState _gridState;
        private MyceliumState _myceliumState;
        private MyceliumBuildingType _myceliumBuildingType;
        
        public Vector2 Coordinate => _coordinate;

        public void Setup(Vector2 coordinate, GridState gridState, MyceliumState myceliumState = MyceliumState.None, MyceliumBuildingType myceliumBuildingType = MyceliumBuildingType.None)
        {
            _coordinate  = coordinate;
            _gridState = gridState;
            _myceliumState = myceliumState;
            _myceliumBuildingType = myceliumBuildingType;
        }
        
        public void ChangeGridState(GridState gridState)
        {
            var oldState = _gridState;
            if (oldState == _gridState) return;
            
            _gridState = gridState;
            if (_gridState == GridState.Occupied)
            {
                _myceliumState = MyceliumState.Standard;
            }
        }

        public void ChangeMyceliumState(MyceliumState myceliumState)
        {
            _myceliumState = myceliumState;
        }

        public Vector2 GetDimension()
        {
            switch (GridTileType)
            {
                case GridTileType.Ground:
                    return new Vector2(2, 2);
                case GridTileType.Wall:
                    return new Vector2(2, 1);
            }
            return Vector2.one;
        }

        public Vector2 GetOffset()
        {
            switch (GridTileType)
            {
                case GridTileType.Wall:
                    return new Vector2(0, 0.5f);
            }
            
            return Vector2.zero;
        }
    }

    public enum GridState
    {
        //non-sight to grid tile
        Locked,
        //sight, but no mycelium
        Unlocked,
        //mycelium on tile
        Occupied
    }

    [Serializable]
    public enum GridTileType
    {
        Ground,
        Wall
    }

    public enum MyceliumState
    {
        None,
        Standard,
        Building
    }

    public enum MyceliumBuildingType
    {
        None,
        Bridge,
        Tower
    }
}
