using System.Collections;
using System.Linq;
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
    public GameObject PreviewBuilding;
    public LayerMask GroundLayer;
    public Button BuildButton;
    public Image BuildButtonImage;
    public Image BuildingImage;
    public Material TransparantMat;
    public Material Greymat;
    public TextMeshProUGUI ResourceText;
    public AudioSource AudioSource;
    public AudioClip BuildingSound;

    private GameObject buildingToPlace;
    private GameObject currentPreview;

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
        if (buildingToPlace == null)
            return;

        HandlePlacement();
    }

    public void BuildBuilding()
    {
        StartPlacing(Building, PreviewBuilding);
    }

    private void StartPlacing(GameObject buildingPrefab, GameObject previewPrefab)
    {
        buildingToPlace = Building;
        currentPreview = Instantiate(PreviewBuilding);
    }

    private void HandlePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, GroundLayer))
        {
            currentPreview.transform.position = hit.point;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(hit.point);
            }
        }
    }

    private void PlaceBuilding(Vector3 pos)
    {
        GameObject placedBuilding = Instantiate(buildingToPlace, pos + new Vector3(0, 0.5f, 0), Quaternion.identity);

        Destroy(currentPreview);
        buildingToPlace = null;

        // Start the build process for the placed building
        StartCoroutine(BuildBuildingCoroutine(placedBuilding));
    }

    public IEnumerator BuildBuildingCoroutine(GameObject building)
    {
        Debug.Log("Start build");
        GatheredResources = 0f;
        building.SetActive(true);
        building.GetComponent<MeshRenderer>().material = TransparantMat;
        Image[] images = building.GetComponentsInChildren<Image>();
        BuildingImage = images.Where(x => x.name == "foreground").FirstOrDefault();
        AudioSource.Play();
        yield return StartCoroutine(ExtractResources());

        Debug.Log("end build");
        AudioSource.Stop();
        building.GetComponent<MeshRenderer>().material = Greymat;
        BuildingImage.fillAmount = 1;
        building.GetComponentInChildren<Canvas>().enabled = false;
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
