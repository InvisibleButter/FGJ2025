using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Grid
{
    public class GridHelper : MonoBehaviour
    {
        public List<GridEntity> GridTiles;
        
        [ContextMenu("Setup GridTiles")]
        public void SetGridTiles()
        {
            GridTiles = new List<GridEntity>();
            GridTiles.Clear();
            
            var children = GetComponentsInChildren<GridEntity>();
            for (int i = 0; i < children.Length; i++)
            {
                var tile = children[i];
                tile.Setup(i, GridState.Locked);
                GridTiles.Add(tile);
            }
        }
    }
}