using System.ComponentModel;
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


    [Space(10), Header("TextFields")]
    [SerializeField] private TextMeshProUGUI _currentStorageText;
    [SerializeField] private TextMeshProUGUI _maxStorageText;

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
            Debug.Log($"Using up {amount} resources");
        }
        else
        {
            Debug.Log($"Not enough resources present");
        }
    }
}
