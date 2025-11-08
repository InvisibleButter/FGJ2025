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
            foreach (var tile in children)
            {
                tile.Setup(CalculateCoordinate(tile), GridState.Locked);
                GridTiles.Add(tile);
            }
        }

        private Vector2 CalculateCoordinate(GridEntity gridEntity)
        {
            var pos = gridEntity.gameObject.transform.position;
            var dimension = gridEntity.GetDimension();
            var offset = dimension / 2;
            float epsilon = 0.0001f;
            Debug.Log("** ofsset " + offset);

            var coordinates = new Vector2(Mathf.FloorToInt((pos.x + offset.x - epsilon) / dimension.x), Mathf.FloorToInt((pos.z + offset.y- epsilon) / dimension.y));
            Debug.Log("*** coordinates: " + coordinates);
            return coordinates;
        }
    }
}