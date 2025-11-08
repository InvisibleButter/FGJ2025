using UnityEngine;

namespace Scripts.Shrooms
{
    public class WatcherShroomEntity : MonoBehaviour
    {
        public float detectionRadius = 10f;      // How far the cone reaches
        public float detectionAngle = 45f;       // Half-angle of the cone (e.g., 45° means 90° total spread)
        public LayerMask targetLayer;            // Which layers count as "entities"
        public Transform forwardTransform;       // Optional: direction source (e.g., player camera or weapon)

        void Update()
        {
            DetectEntities();
        }

        void DetectEntities()
        {
            // Step 1: Get all colliders in a sphere around you
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);

            // Step 2: Check which ones are inside the cone
            foreach (Collider hit in hits)
            {
                Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;

                // Determine what direction the cone faces
                Vector3 forwardDir = forwardTransform ? forwardTransform.forward : transform.forward;

                // Step 3: Check angle
                float angleToTarget = Vector3.Angle(forwardDir, directionToTarget);

                if (angleToTarget <= detectionAngle)
                {
                    // Optionally: check line of sight
                    if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit rayHit, detectionRadius))
                    {
                        if (rayHit.collider == hit)
                        {
                            Debug.Log($"Detected entity: {hit.name}");
                            Debug.DrawLine(transform.position, hit.transform.position, Color.green);
                        }
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                }
            }
        }
        
    }
}