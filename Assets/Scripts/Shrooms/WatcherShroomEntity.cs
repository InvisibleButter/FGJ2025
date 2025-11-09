using Scripts.Grid;
using UnityEngine;

namespace Scripts.Shrooms
{
    public class WatcherShroomEntity : MonoBehaviour
    {
        public float detectionRadius = 10f;      // How far the cone reaches
        public float detectionAngle = 45f;       // Half-angle of the cone (e.g., 45° means 90° total spread)
        public LayerMask targetLayer;            // Which layers count as "entities"
        public Transform forwardTransform;       // Optional: direction source (e.g., player camera or weapon)
        
        public void RefreshView()
        {
           // DetectEntities();
        }

        void DetectEntities()
        {
            // Step 1: Get all colliders in a sphere around you
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);

            Vector3 origin = transform.position + new Vector3(0, 0.5f, 0);
            Vector3 forwardDir = forwardTransform ? forwardTransform.forward : transform.forward;
            
            foreach (Collider hit in hits)
            {
                var directionToTarget = (hit.transform.position - origin).normalized;
                var angleToTarget = Vector3.Angle(forwardDir, directionToTarget);

                // Check if inside the detection cone
                if (!(angleToTarget <= detectionAngle)) continue;
                var gridEntity = hit.GetComponent<GridEntity>();
                if (gridEntity == null) continue;
                if (gridEntity.GridState != GridState.Locked)
                {
                    continue;
                }
                // Check line of sight
                if (!Physics.Raycast(origin, directionToTarget, out RaycastHit rayHit, detectionRadius)) continue;
                    
                var otherEntity = rayHit.collider.GetComponent<GridEntity>();
                
                // Only count it as detected if the ray actually hits this same collider
                if (rayHit.collider == hit || (otherEntity != null && otherEntity.GridTileType == GridTileType.Ground))
                {
                    if (gridEntity == null) continue;
                    gridEntity.ChangeGridState(GridState.Unlocked);
                    Debug.Log($"✅ Detected visible entity: {hit.name}");
                    // Debug.DrawLine(origin, hit.transform.position, Color.green);
                }
                else
                {
                    // Something else is blocking the line of sight
                    Debug.Log($"🚫 {hit.name} is behind {rayHit.collider.name}");
                    Debug.DrawLine(origin, hit.transform.position, Color.red);
                }
            }
        }
        
    }
}