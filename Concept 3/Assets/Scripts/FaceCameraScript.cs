using UnityEngine;

public class FaceCameraScript : MonoBehaviour
{
    [SerializeField]
    private Camera _targetCamera;
    void Start()
    {
        if (_targetCamera == null)
        {
            _targetCamera = Camera.main; // Automatically use main camera if not assigned
        }
    }

    void LateUpdate()
    {
        // Make the canvas face the camera
        transform.LookAt(transform.position + _targetCamera.transform.rotation * Vector3.forward, _targetCamera.transform.rotation * Vector3.up);
    }

}
