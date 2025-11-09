
using Scripts.Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Shrooms
{
    public class WatcherAbility : IShroomAbility
    {
        private float _watcherAngle = 45f;
        private Camera _watcherCam; 
        private LayerMask _layerMask;
        
        public WatcherAbility (Camera watcherCam, LayerMask layerMask)
        {
            _watcherCam = watcherCam;
            _layerMask = layerMask;
        }
        
        public void Execute()
        {
            //ServiceLocator.Instance.GetService<ShroomAbilityService>().AddWatcher(_startCoordinate, _watchRotation);
            ActivateAbility(_watcherCam);
        }
        
        public void ActivateAbility(Camera watcherCam)
            {
                var gridService= ServiceLocator.Instance.GetService<ShroomGridService>();
                var blockList=gridService.GetBlockList();
        
                foreach (var block in blockList)
                {
                    if (IsInView(block.GameObject()))
                    {
                        block.ChangeGridState(GridState.Unlocked);
                    }
                }
            }
            
            private bool IsInView(GameObject toCheck)
            {
                Vector3 pointOnScreen = _watcherCam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);
        
                //Is in front
                if (pointOnScreen.z < 0)
                {
                    //Debug.Log("Behind: " + toCheck.name);
                    return false;
                }
        
                //Is in FOV
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_watcherCam);
                Collider collider = toCheck.GetComponent<Collider>();
                
                
                if (!GeometryUtility.TestPlanesAABB(planes , collider.bounds))
                    return false;
                
                /*if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
                    (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
                {
                    Debug.Log("OutOfBounds: " + toCheck.name);
                    return false;
                }*/
        
                RaycastHit hit;
                Vector3 heading = toCheck.transform.position - _watcherCam.transform.position;
                Vector3 direction = heading.normalized;// / heading.magnitude;
                
                if (Physics.Raycast(_watcherCam.transform.position,direction, out hit, _watcherCam.farClipPlane, _layerMask, QueryTriggerInteraction.Ignore ))
                {
                    if (hit.transform.name != toCheck.name)
                    {
                        //Debug.DrawLine(_watcherCam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red,10000f);
                        //Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                        //Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                        Debug.Log("CHECKING CLOSER INSPECTION FOR: " + toCheck.name);
                        return CloserInspection(toCheck.GetComponent<GridEntity>());
                    }
                }
                return true;
            }

            private bool CloserInspection(GridEntity gridEntity,bool showDebugLines=false)
            {
                var vertecies = gridEntity.GetVertices();
                var worldPosition = _watcherCam.transform.position;
                foreach (var vertex in vertecies)
                {
                    RaycastHit hit;
                    Vector3 heading = vertex - worldPosition;
                    Vector3 direction = heading.normalized;// / heading.magnitude;
                    if (Physics.Raycast(worldPosition,direction,out hit, _watcherCam.farClipPlane, _layerMask, QueryTriggerInteraction.Ignore ))
                    {
                        if (hit.transform.name != gridEntity.transform.name)
                        {
                            if (showDebugLines)
                                Debug.DrawLine(_watcherCam.transform.position, vertex, Color.green, 10000f);
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            
           
    }
}