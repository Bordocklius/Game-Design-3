using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance {  get; private set; }

    public float BuildingCost;
    public float GatheredResources;
    public float ResourceGatheringRate = 1f;
    public GameObject[] resourceGatherers;

    public GameObject Building;

    public Button BuildButton;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Build (cost {BuildingCost})";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildBuilding()
    {
        StartCoroutine(BuildBuildingCoroutine());
    }

    public IEnumerator BuildBuildingCoroutine()
    {
        Debug.Log("Start build");
        GatheredResources = 0f;
        yield return StartCoroutine(ExtractResources());
        Debug.Log("end build");
        Building.SetActive(true);
    }

    public IEnumerator ExtractResources()
    {
        while(GatheredResources < BuildingCost)
        {
            foreach(GameObject obj in resourceGatherers)
            {
                if(obj.TryGetComponent<IResourceGatherer>(out IResourceGatherer resourceGatherer))
                {
                    GatheredResources += resourceGatherer.ExtractResources();
                }
            }
            Debug.Log("Resources gathered: " + GatheredResources);
            yield return new WaitForSeconds(ResourceGatheringRate);
        }
    }
}
