using System.Collections;
using UnityEngine;

public class BuildingRadius : MonoBehaviour, IResourceGatherer
{
    public float Radius;
    public LayerMask ResourceNodes;
    public float RatePerSec;
    public BuildingManager BuildingManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    public float ExtractResources()
    {
        float gatheredResources = 0f;
        Collider[] cols = Physics.OverlapSphere(transform.position, Radius, ResourceNodes);
        foreach(Collider col in cols)
        {
            if(col.gameObject.TryGetComponent<ResourceNode>(out ResourceNode resourceNode))
            {
                gatheredResources = resourceNode.Extract(RatePerSec);
                break;
            }
        }
        return gatheredResources;
    }
}
