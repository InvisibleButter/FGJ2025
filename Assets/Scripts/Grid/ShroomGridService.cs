using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Shrooms;
using UnityEngine;

namespace Scripts.Grid
{
    public class ShroomGridService : MonoBehaviour, IService
    {
        [SerializeField] private int _maxChargedDistance = 5;
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
                    var tile = allTiles.FirstOrDefault(e=> e.GetCoordinate() == new Vector2Int(x, y));
                    if (tile != null)
                    {
                        result.Add(tile);
                    }
                }
            }
            
            return result;
        }

        public List<GridEntity> GetTilesHitByCharge(Vector2 startCoord, Vector3 worldForward, float chargeDistance)
        {
            // 1) Forward → grid direction
            Vector2 forward2D = new Vector2(worldForward.x, worldForward.z).normalized;

            var gridDir = Mathf.Abs(forward2D.x) > Mathf.Abs(forward2D.y) ? new Vector2Int(forward2D.x > 0 ? 1 : -1, 0) : new Vector2Int(0, forward2D.y > 0 ? 1 : -1);

            var result = new List<GridEntity>();

            for (int i = 1; i <= chargeDistance; i++)
            {
                Vector2 targetCoord = startCoord + gridDir * i;

                GridEntity tile = FindTileByCoord(targetCoord, _gridHelper.GridTiles);
                if (tile != null)
                {
                    result.Add(tile);
                    if (tile.GridTileType == GridTileType.Wall)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private GridEntity FindTileByCoord(Vector2 coord, List<GridEntity> allTiles)
        {
            foreach (var tile in allTiles)
            {
                if (tile.GetCoordinate() == coord)
                    return tile;
            }
            return null;
        }
        
        public WatcherShroomEntity AddWatcherShroom(Vector2 gridIndex, Vector3 rotation)
        {
            var targetGridTile = _gridHelper.GridTiles.FirstOrDefault(e=> e.GetCoordinate() == gridIndex);
            Quaternion spawnRot = Quaternion.LookRotation(rotation);
            var watcher = Instantiate(_watcherShroomPrefab, targetGridTile.gameObject.transform.position + new Vector3(0, 0.5f, 0), spawnRot);
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

        public List<GridEntity> GetBlockList()
        {
            return _gridHelper.GridTiles;
        }

        public void UpDateAllCells(bool isWatcher)
        {
            foreach (var tile in _gridHelper.GridTiles)
            {
                tile.UpdateVisuals(isWatcher);
            }
        }
    }
}