using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GameManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("StorageManager");
                    _instance = obj.AddComponent<GameManager>();
                }

            }

            return _instance;
        }
        private set { _instance = value; }
    }

    [Space(10), Header("Building Placement")]
    public LayerMask GroundLayer;
    private GameObject _buildingToPlace;
    private GameObject _currentPreviewBuilding;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_buildingToPlace == null)
            return;

        HandleBuildingPlacement();
    }

    public void StartPlacing(GameObject buildingPrefab, GameObject previewPrefab)
    {
        _buildingToPlace = buildingPrefab;
        _currentPreviewBuilding = Instantiate(previewPrefab);
    }

    private void HandleBuildingPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, GroundLayer))
        {
            _currentPreviewBuilding.transform.position = hit.point;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(hit.point);
            }
        }
    }

    private void PlaceBuilding(Vector3 position)
    {
        GameObject placedBuilding = Instantiate(_buildingToPlace, position + new Vector3(0, 0.5f, 0), Quaternion.identity);

        Destroy(_currentPreviewBuilding);
        _buildingToPlace = null;
    }

    public void DestroyRandomStorage()
    {
        StorageManager.Instance.DestroyRandomStorageBuilding();
    }
}
