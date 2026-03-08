using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorageBuilding : MonoBehaviour
{

    [Space(10), Header("Settings")]
    public float StorageCapacity;

    private float _storedResources;
    public float StoredResources
    {
        get { return _storedResources; }
        set
        {
            _storedResources = value;

            if (_storedResources > StorageCapacity)
                _storedResources = StorageCapacity;
            if(_storedResources < 0)
                _storedResources = 0;

            UpdateText();
        }
    }

    [SerializeField] private TextMeshProUGUI _storageText;


    [Space(10), Header("Resource packets")]
    [SerializeField] private GameObject _resourcePacketPrefab;
    [SerializeField, Range(0.5f, 1f)] private float _resourcesLost;
    [SerializeField] private float _spawnSpread;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StorageManager.Instance.AddStorageBuilding(this);
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateText()
    {
        _storageText.text = $"{_storedResources}";
    }

    private void OnDestroy()
    {
        SpawnResourcePackets();

        // Preserve resources left
        float keptResources = StoredResources * (1f - _resourcesLost);
        Debug.Log($"Resources kept: {keptResources}");
        StorageManager.Instance.RemoveStorageBuilding(this);
        if(keptResources > 0f)
            StorageManager.Instance.AddResources(keptResources);
    }

    private void SpawnResourcePackets()
    {
        float packetMax = _resourcePacketPrefab.GetComponent<ResourcePacket>().MaxPacketValue;
        float availableResources = StoredResources * _resourcesLost;
        int maxPacketsToSpawn = Mathf.CeilToInt(StorageCapacity / packetMax);


        // Build list of packet values (maxed out packets with possibly one remainder)
        List<float> values = new List<float>();
        while(availableResources > 0f && values.Count < maxPacketsToSpawn)
        {
            float value = Mathf.Min(packetMax, availableResources);
            values.Add(value);
            availableResources -= value;
        }

        Debug.Log($"Spawning {values.Count} packets");

        foreach(var value in values)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-_spawnSpread, _spawnSpread), 0.2f, Random.Range(-_spawnSpread, _spawnSpread));

            GameObject obj = Instantiate(_resourcePacketPrefab, spawnPos, Quaternion.identity);
            ResourcePacket packet = obj.GetComponent<ResourcePacket>();
            packet.PacketValue = value;
        }
    }
}
