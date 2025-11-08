using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveService : IService
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
