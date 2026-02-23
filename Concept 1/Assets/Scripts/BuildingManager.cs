using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.HableCurve;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance {  get; private set; }

    public float BuildingCost;
    public float GatheredResources;
    public float ResourceGatheringRate = 1f;
    public GameObject[] resourceGatherers;

    public GameObject Building;

    public Button BuildButton;
    public Image BuildButtonImage;
    public Image BuildingImage;
    public Material TransparantMat;
    public Material Greymat;
    public TextMeshProUGUI ResourceText;
    
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
        Building.SetActive(true);
        Building.GetComponent<MeshRenderer>().material = TransparantMat;
        yield return StartCoroutine(ExtractResources());
        Debug.Log("end build");
        Building.GetComponent<MeshRenderer>().material = Greymat;
        BuildingImage.fillAmount = 1;
        Building.GetComponentInChildren<Canvas>().enabled = false;
    }

    public IEnumerator ExtractResources()
    {
        while(GatheredResources < BuildingCost)
        {
            foreach (GameObject obj in resourceGatherers)
            {
                if(obj.TryGetComponent<IResourceGatherer>(out IResourceGatherer resourceGatherer))
                {
                    GatheredResources += resourceGatherer.ExtractResources();                    
                }
            }
            Debug.Log("Resources gathered: " + GatheredResources);
            SetResourceText();
            if (GatheredResources >= BuildingCost)
            {
                GatheredResources = BuildingCost;
                break;
            }
            BuildButtonImage.fillAmount = GatheredResources / BuildingCost;
            BuildingImage.fillAmount = GatheredResources / BuildingCost;
            yield return new WaitForSeconds(ResourceGatheringRate);
        }
    }

    private void SetResourceText()
    {
        ResourceText.text = $"{GatheredResources}/{BuildingCost}";
    }
}
