using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ServiceLocator _serviceLocator;

    private void Awake()
    {
        RegisterServices();
    }

    private void RegisterServices()
    {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.OnAllInitilized += OnAllInitialized;

        var services = GetComponentsInChildren<IService>();
        foreach(var service in services)
        {
            _serviceLocator.Add(service);
        }
        
        _serviceLocator.InitializeAll();
    }

    protected virtual void AddAdditionalServices() { }
    
    protected virtual void OnAllInitialized() { }
}
