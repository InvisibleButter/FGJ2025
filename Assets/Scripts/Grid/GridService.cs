using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Grid
{
    public class GridService : MonoBehaviour, IService
    {
        [SerializeField] private GridHelper _gridHelper;
        
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
