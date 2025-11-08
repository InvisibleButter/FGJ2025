using System;
using System.Collections.Generic;

public class ServiceLocator
{
    private static ServiceLocator locator = null;

    public static ServiceLocator Instance
    {
        get
        {
            if (locator == null)
            {
                locator = new ServiceLocator();
            }
            return locator;
        }
    }

    public Action OnAllInitilized { get; set; }

    private ServiceLocator()
    {
    }

    private Dictionary<Type, IService> _registeredServices = new Dictionary<Type, IService>();

    public void Register<T>(IService serviceInstance) where T : IService
    {
        _registeredServices[typeof(T)] = serviceInstance;
    }

    public T GetService<T>() where T : IService
    {
        T serviceInstance = (T)_registeredServices[typeof(T)];
        return serviceInstance;
    }

    public void Add(IService instance)
    {
        _registeredServices[instance.GetType()] = instance;
    }


    public void InitializeAll() 
    {
        foreach(var service in _registeredServices)
        {
            service.Value.Initialize();
        }

        OnAllInitilized?.Invoke();
    }
}
