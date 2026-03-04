using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    private Button _button;
    private Image _image;

    [Space(10), Header("Assigned building")]
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private GameObject _previewPrefab;
    [SerializeField] private float _buildingCost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Button_OnPress);

        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if(StorageManager.Instance.CurrentStoredResources < _buildingCost)
        {
            _image.color = Color.gray;
        }
        else
        {
            _image.color = Color.lightGray;
        }
    }

    private void Button_OnPress()
    {
        if(StorageManager.Instance.CurrentStoredResources >= _buildingCost)
        {  
            StorageManager.Instance.UseResources(_buildingCost);
            GameManager.Instance.StartPlacing(_buildingPrefab, _previewPrefab);
        }
    }
}
