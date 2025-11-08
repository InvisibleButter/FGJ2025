using UnityEngine;

public class EntityService : MonoBehaviour, IService
{
    public bool IsInitialized { get; set; }

    public void DeInitialize()
    {
        IsInitialized = false;
    }

    public void Initialize()
    {
        IsInitialized = true;
    }
}
