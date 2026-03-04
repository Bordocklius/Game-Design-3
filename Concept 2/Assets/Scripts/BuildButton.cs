using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    private Button _button;

    [Space(10), Header("Assigned building")]
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private GameObject _previewPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Button_OnPress);
    }

    private void Button_OnPress()
    {
        GameManager.Instance.StartPlacing(_buildingPrefab, _previewPrefab);
    }
}
