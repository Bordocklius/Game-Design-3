using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    private static StorageManager _instance;
    public static StorageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<StorageManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("StorageManager");
                    _instance = obj.AddComponent<StorageManager>();
                }

            }

            return _instance;
        }
        private set { _instance = value; }
    }


    [SerializeField] private float _maxStorageCapacity;
    public float MaxStorageCapacity
    {
        get { return _maxStorageCapacity; }
        set
        {
            _maxStorageCapacity = value;
            UpdateMaxStorageText();
        }
    }

    [SerializeField] private float _currentStoredResources;
    public float CurrentStoredResources
    {
        get { return _currentStoredResources; }
        set
        {
            _currentStoredResources = value;

            if (_currentStoredResources > MaxStorageCapacity)
                _currentStoredResources = MaxStorageCapacity;

            UpdateCurrentStorageText();
        }
    }

    [SerializeField] private List<StorageBuilding> _storageBuildings;

    [Space(10), Header("TextFields")]
    [SerializeField] private TextMeshProUGUI _currentStorageText;
    [SerializeField] private TextMeshProUGUI _maxStorageText;

    [Space(10), Header("Resource generation")]
    [SerializeField] private float _resourcesPerTick;
    [SerializeField] private float _resourceTickRate;
    private float _timer = 0f;

    [SerializeField, Range(0.5f,1f)] private float _resourcesLostModifier;

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
        UpdateCurrentStorageText();
        UpdateMaxStorageText();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _resourceTickRate)
        {
            AddResources(_resourcesPerTick);
            StorageBuilding storage = _storageBuildings.Where(x => x.StoredResources < x.StorageCapacity).FirstOrDefault();
            if(storage != null)
                storage.StoredResources += _resourcesPerTick;
            _timer -= _resourceTickRate;
        }
    }

    public void AddStorageBuilding(StorageBuilding storageBuilding)
    {
        _storageBuildings.Add(storageBuilding);
        MaxStorageCapacity += storageBuilding.StorageCapacity;
    }

    public void RemoveStorageBuilding(StorageBuilding storageBuilding)
    {
        _storageBuildings.Remove(storageBuilding);
        MaxStorageCapacity -= storageBuilding.StorageCapacity;
        CurrentStoredResources -= storageBuilding.StoredResources * _resourcesLostModifier;
    }

    public void DestroyRandomStorageBuilding()
    {
        int randomIndex = Random.Range(0, _storageBuildings.Count);
        StorageBuilding obj = _storageBuildings[randomIndex];
        Destroy(obj.gameObject);
    }

    private void UpdateMaxStorageText()
    {
        _maxStorageText.text = MaxStorageCapacity.ToString();
    }

    private void UpdateCurrentStorageText()
    {
        _currentStorageText.text = CurrentStoredResources.ToString();
    }

    public void AddResources(float amount)
    {
        CurrentStoredResources += amount;
    }

    public void UseResources(float amount)
    {
        if(CurrentStoredResources >= amount)
        {
            CurrentStoredResources -= amount;
            StorageBuilding storage = _storageBuildings.Where(x => x.StoredResources < x.StorageCapacity).FirstOrDefault();
            if (storage != null)
                storage.StoredResources -= amount;
            Debug.Log($"Using up {amount} resources");
        }
        else
        {
            Debug.Log($"Not enough resources present");
        }
    }
}
