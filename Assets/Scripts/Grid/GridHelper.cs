using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Scripts.Grid
{
    public class GridHelper : MonoBehaviour
    {
        public List<GridEntity> GridTiles;

        [SerializeField] private bool isDebugMode;
        
        [ContextMenu("Setup GridTiles")]
        public void SetGridTiles()
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Setup Values");   // allow undo + mark for change tracking
#endif
      
            Debug.Log("Setup GridTiles");
            GridTiles = new List<GridEntity>();
            GridTiles.Clear();

            int count = 0;
            var children = GetComponentsInChildren<GridEntity>();
            foreach (var tile in children)
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Setup Values");   // allow undo + mark for change tracking
#endif
                
                tile.Setup(CalculateCoordinate(tile), GridState.Locked, debugMode: isDebugMode);
                GridTiles.Add(tile);
                count++;
#if UNITY_EDITOR
                EditorUtility.SetDirty(tile);              // ensure Unity saves the new values
#endif
            }
            
            Debug.Log($"Finish setup {count} GridTiles");
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);              // ensure Unity saves the new values
#endif
        }

        private Vector2 CalculateCoordinate(GridEntity gridEntity)
        {
            var pos = gridEntity.gameObject.transform.position;
            var dimension = gridEntity.GetDimension();
            var offset = dimension / 2;
            var epsilon = 0.0001f;

            var coordinates = new Vector2(Mathf.FloorToInt((pos.x + offset.x - epsilon) / dimension.x), Mathf.FloorToInt((pos.z +offset.y - epsilon) / dimension.y));
            return coordinates;
        }
    }
}