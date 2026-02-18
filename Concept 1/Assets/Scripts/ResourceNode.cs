using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public float Value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Value <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
