using UnityEngine;

public class StorageBuilding : MonoBehaviour
{

    [Space(10), Header("Settings")]
    [SerializeField] private float _addedCapacity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StorageManager.Instance.MaxStorageCapacity += _addedCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
