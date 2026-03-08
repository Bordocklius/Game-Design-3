using UnityEngine;

public class ResourcePacket : MonoBehaviour
{
    public float PacketValue;
    public float MaxPacketValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnMouseDown()
    {
        AddResourcesToPool();
        Destroy(this.gameObject);
    }

    private void AddResourcesToPool()
    {
        StorageManager.Instance.AddResources(PacketValue);
    }
}
