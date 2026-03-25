using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField, Min(0f)] private float _followSpeed = 5f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, -3f);

    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        _offset = transform.position - _playerTransform.position;   
    }

    private void LateUpdate()
    {
        if (_playerTransform == null)
            return;

        Vector3 targetPosition = _playerTransform.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, 1f / _followSpeed);
    }
}