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
    [SerializeField] private GameObject _resourcePacketPrefab;
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

        StorageManager.Instance.RemoveStorageBuilding(this);
    }

    private void SpawnResourcePackets()
    {
        float packetMax = _resourcePacketPrefab.GetComponent<ResourcePacket>().MaxPacketValue;
        int maxPacketsToSpawn = Mathf.CeilToInt(StorageCapacity / packetMax);
        float availableResources = StoredResources;

        // Build list of packet values (maxed out packets with possibly one remainder)
        List<float> values = new List<float>();
        while(availableResources > 0f && values.Count < maxPacketsToSpawn)
        {
            float value = Mathf.Min(packetMax, availableResources);
            values.Add(value);
            availableResources -= value;
        }

        // If we hit the max packet limit but there's still remaining resources,
        // fold the remainder into the last packet (so nothing is lost).
        if (availableResources > 0f && values.Count > 0)
        {
            values[values.Count - 1] += availableResources;
            availableResources = 0f;
        }

        foreach(var value in values)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-_spawnSpread, _spawnSpread), 0.2f, Random.Range(-_spawnSpread, _spawnSpread));

            GameObject obj = Instantiate(_resourcePacketPrefab, spawnPos, Quaternion.identity);
            ResourcePacket packet = obj.GetComponent<ResourcePacket>();
            packet.PacketValue = value;
        }
    }
}
