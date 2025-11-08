using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public GameObject PlantDummy;
    public Transform Holder;
    public List<GameObject> Plants;

    public void Setup()
    {  
        LoadFromConfig();     
    }

    private void LoadFromConfig()
    {
        Plants = new List<GameObject>();
        Cell[,] grid = ServiceLocator.Instance.GetService<GridService>().Grid;
        foreach (var entity in ServiceLocator.Instance.GetService<MapLoaderService>().MapConfig.Entities)
        {
           
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ServiceLocator.Instance.GetService<MapLoaderService>().WriteMap();
        }
    }
}