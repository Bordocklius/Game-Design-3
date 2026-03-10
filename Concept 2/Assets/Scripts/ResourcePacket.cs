using UnityEngine;

public class ResourcePacket : MonoBehaviour
{
    public float PacketValue;
    public float MaxPacketValue;

    public float DecayTimer;
    private float _timer;
    public float DecayValue;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= DecayTimer)
        {
            PacketValue -= DecayValue;
            if (PacketValue <= 0)
            {
                Destroy(this.gameObject);
                return;
            }
            _timer = 0;            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnMouseDown()
    {
        if (!StorageManager.Instance.HasRoomForStorage(PacketValue))
            return;

        AddResourcesToPool();
        Destroy(this.gameObject);
    }

    private void AddResourcesToPool()
    {
        StorageManager.Instance.AddResources(PacketValue);
        Debug.Log($"Adding {PacketValue}");
    }
}
