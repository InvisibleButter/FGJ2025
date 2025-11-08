
using Scripts.Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Shrooms
{
    public class WatcherAbility : IShroomAbility
    {
        private Vector2 _startCoordinate;
        private Vector3 _watchRotation;
        private float _watcherAngle = 45f;
        private Camera _watcherCam; 
        private LayerMask _layerMask;
        
        public WatcherAbility ( Vector3 watcherDirection, Camera watcherCam, LayerMask layerMask)
        {
            _startCoordinate = watcherCam.transform.position;
            _watchRotation = watcherDirection;
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
                    Debug.Log("Behind: " + toCheck.name);
                    return false;
                }
        
                //Is in FOV
                if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
                    (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
                {
                    Debug.Log("OutOfBounds: " + toCheck.name);
                    return false;
                }
        
                RaycastHit hit;
                Vector3 heading = toCheck.transform.position - _watcherCam.transform.position;
                Vector3 direction = heading.normalized;// / heading.magnitude;
                
                if (Physics.Raycast(_watcherCam.transform.position,direction, out hit, _watcherCam.farClipPlane, _layerMask, QueryTriggerInteraction.Ignore ))
                {
                    if (hit.transform.name != toCheck.name)
                    {
                        
                        Debug.DrawLine(_watcherCam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red,10000f);
                        Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                        
                        Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                        return false;
                    }
                }
                return true;
            }
    }
}