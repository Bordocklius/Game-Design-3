using TMPro;
using UnityEngine;

public class StorageBuilding : MonoBehaviour
{

    [Space(10), Header("Settings")]
    public float StorageCapacity;

    private float _storedResources;
    public float StoredResources
    {
        get { return _storedResources; }
        set
        {
            _storedResources = value;

            if (_storedResources > StorageCapacity)
                _storedResources = StorageCapacity;
            if(_storedResources < 0)
                _storedResources = 0;

            UpdateText();
        }
    }

    [SerializeField] private TextMeshProUGUI _storageText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StorageManager.Instance.AddStorageBuilding(this);
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateText()
    {
        _storageText.text = $"{_storedResources}";
    }

    private void OnDestroy()
    {
        StorageManager.Instance.RemoveStorageBuilding(this);
    }
}
