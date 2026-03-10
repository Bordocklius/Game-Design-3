using System.Collections.Generic;
using UnityEngine;

public class DecoyStorage : MonoBehaviour
{

    [Space(10), Header("Decoy settings")]
    public float StoredResources;
    public float StorageCapacity;

    [Space(10), Header("Packet settings")]
    [SerializeField] private GameObject _resourcePacketPrefab;
    [SerializeField, Range(0.5f, 1f)] private float _resourcesLost;
    [SerializeField] private float _spawnSpread;

    private void OnDestroy()
    {
        SpawnRandomPackets();
    }

    private void SpawnRandomPackets()
    {
        float packetMax = _resourcePacketPrefab.GetComponent<ResourcePacket>().MaxPacketValue;
        float availableResources = StoredResources * _resourcesLost;
        int maxPacketsToSpawn = Mathf.CeilToInt(StorageCapacity / packetMax); 

        // Spawn at least 1 packet
        int randomPacketsToSpawn = Random.Range(1, maxPacketsToSpawn);

        // Build list of packet values (maxed out packets with possibly one remainder)
        List<float> values = new List<float>();
        while (availableResources > 0f && values.Count < randomPacketsToSpawn)
        {
            float value = Mathf.Min(packetMax, availableResources);
            values.Add(value);
            availableResources -= value;
        }

        Debug.Log($"Spawning {values.Count} packets");

        foreach (var value in values)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-_spawnSpread, _spawnSpread), 0.2f, Random.Range(-_spawnSpread, _spawnSpread));

            GameObject obj = Instantiate(_resourcePacketPrefab, spawnPos, Quaternion.identity);
            ResourcePacket packet = obj.GetComponent<ResourcePacket>();
            packet.PacketValue = value;
        }
    }
}
