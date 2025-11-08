using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Shrooms;
using UnityEngine;

namespace Scripts.Grid
{
    public class ShroomGridService : MonoBehaviour, IService
    {
        [SerializeField] private GridHelper _gridHelper;
        [SerializeField] private WatcherShroomEntity _watcherShroomPrefab;

        public List<GridEntity> GetAdjacentTilesOfState(Vector2 startCoordinate, GridState state)
        {
            var allTiles = _gridHelper.GridTiles.Where(e => e.GridTileType == GridTileType.Ground && e.GridState == state).ToList();
            var result = new List<GridEntity>();
            for (int x = (int)startCoordinate.x - 1; x <= startCoordinate.x + 1; x++)
            {
                for (int y = (int)startCoordinate.y - 1; y <= startCoordinate.y + 1; y++)
                {
                    var tile = allTiles.FirstOrDefault(e=> e.Coordinate == new Vector2Int(x, y));
                    if (tile != null)
                    {
                        result.Add(tile);
                    }
                }
            }
            
            return result;
        }

        public WatcherShroomEntity AddWatcherShroom(Vector2 gridIndex)
        {
            var targetGridTile = _gridHelper.GridTiles.FirstOrDefault(e=> e.Coordinate == gridIndex);
            var watcher = Instantiate(_watcherShroomPrefab, targetGridTile.gameObject.transform);
            targetGridTile.ChangeMyceliumState(MyceliumState.Building, MyceliumBuildingType.Watcher);
            return watcher.GetComponent<WatcherShroomEntity>();
        }
        
        public void Initialize()
        {
            IsInitialized = true;
        }

        public void DeInitialize()
        {
            IsInitialized = false;
        }

        public bool IsInitialized { get; set; }
    }
}