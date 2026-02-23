using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class BuildingRadius : MonoBehaviour, IResourceGatherer
{
    public float Radius;
    public LayerMask ResourceNodes;
    public float RatePerSec;
    public BuildingManager BuildingManager;

    [Space(10), Header("Circle render")]
    public LineRenderer LineRenderer;
    public int Segments = 60;


    private void Awake()
    {
        if (LineRenderer == null)
        {
            LineRenderer = GetComponent<LineRenderer>();
           

        }

        LineRenderer.loop = true;
        LineRenderer.useWorldSpace = true;
        LineRenderer.positionCount = Segments;
        DrawCircle();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawCircle()
    {
        float angleStep = 360f / Segments;

        for (int i = 0; i < Segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            float x = Mathf.Cos(angle) * Radius;
            float z = Mathf.Sin(angle) * Radius;

            LineRenderer.SetPosition(i, new Vector3(x, 0.05f, z));
        }
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
