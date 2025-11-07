using System;
using UnityEngine;

namespace Scripts.Grid
{
    public class GridEntity : MonoBehaviour
    {
        public GridTileType GridTileType;
        
        private int _index;
        private GridState _gridState;
        private MyceliumState _myceliumState;
        private MyceliumBuildingType _myceliumBuildingType;
        
        public int Index => _index;

        public void Setup(int index, GridState gridState, MyceliumState myceliumState = MyceliumState.None, MyceliumBuildingType myceliumBuildingType = MyceliumBuildingType.None)
        {
            _index = index;
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
