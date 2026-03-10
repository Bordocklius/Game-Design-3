using System.Collections.Generic;
using UnityEngine;

public class OverlapArea : MonoBehaviour
{
    public List<GameObject> ContainedObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyAllBuildings()
    {
        foreach(var obj in ContainedObjects)
        {
            Destroy(obj);
        }
        ContainedObjects.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<StorageBuilding>(out StorageBuilding storageBuilding))
        {
            ContainedObjects.Add(storageBuilding.gameObject);
        }
    }
}
