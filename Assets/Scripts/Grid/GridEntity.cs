using System;
using UnityEngine;

namespace Scripts.Grid
{
    public class GridEntity : MonoBehaviour
    {
        public GridTileType GridTileType;
        [SerializeField] private bool isStartMycelium;
        [SerializeField] private BoxCollider blocker;
        [SerializeField] private GameObject myceliumVisual;
        [SerializeField] private MeshRenderer entityRenderer;

        public Vector2 _coordinate;
        public GridState _gridState;
        
        private MyceliumState _myceliumState;
        private MyceliumBuildingType _myceliumBuildingType;
        private bool _isDebugMode;
        
        public Vector2 Coordinate => _coordinate;
        public GridState GridState => _gridState;

        public void Setup(Vector2 coordinate, GridState gridState, MyceliumState myceliumState = MyceliumState.None, MyceliumBuildingType myceliumBuildingType = MyceliumBuildingType.None, bool debugMode = false)
        {
            _isDebugMode = debugMode;
            _coordinate  = coordinate;
            _gridState = isStartMycelium ? GridState.Occupied : gridState;
            _myceliumState = isStartMycelium ? MyceliumState.Standard : myceliumState;
            _myceliumBuildingType = myceliumBuildingType;

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if(myceliumVisual != null)
            {
                myceliumVisual.SetActive(_gridState == GridState.Occupied);
            }

            if (blocker != null)
            {
                blocker.enabled = _gridState is GridState.Locked;
            }

            entityRenderer.enabled = _isDebugMode || _gridState is GridState.Unlocked or GridState.Occupied;
        }

        public void ChangeGridState(GridState gridState)
        {
            var oldState = _gridState;
            if (oldState == gridState) return;
            
            _gridState = gridState;
            if (_gridState == GridState.Occupied)
            {
                _myceliumState = MyceliumState.Standard;
            }
            
            UpdateVisuals();
        }

        public void ChangeMyceliumState(MyceliumState myceliumState)
        {
            _myceliumState = myceliumState;
            
            UpdateVisuals();
        }

        public Vector2 GetDimension()
        {
            switch (GridTileType)
            {
                case GridTileType.Ground:
                    return new Vector2(2, 2);
                case GridTileType.Wall:
                    return new Vector2(2, 2);
            }
            return Vector2.one;
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
