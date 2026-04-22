using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField, Min(0f)] private float _followSpeed = 5f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, -3f);


    [Space(10), Header("Bobbing")]
    [SerializeField] private bool _enableBobbing;
    [SerializeField] private float _bobAmplitude = 0.05f;
    [SerializeField] private float _bobFrequency = 6f;
    [SerializeField] private float _bobSmoothTime = 0.08f;
    [SerializeField] private float _movementThreshold = 0.1f;
    [SerializeField] private float _maxSpeedForBobbing = 6f;

    private Vector3 _velocity = Vector3.zero;

    // bob state
    private float _bobTimer;
    private float _currentBobY;
    private float _bobVelocity;

    // optional character controller for reliable speed
    private CharacterController _playerController;
    private Vector3 _previousPlayerPos;

    private void Awake()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.Find("Player").transform;
        }

        _offset = transform.position - _playerTransform.position;   

        if(_playerController == null && _playerTransform != null )
        {
            _playerController = _playerTransform.GetComponent<CharacterController>();
            _previousPlayerPos = _playerTransform.position;
        }
    }

    private void LateUpdate()
    {
        if (_playerTransform == null)
            return;

        // compute horizontal movement speed (prefer CharacterController.velocity)
        float speed = 0f;
        if (_playerController != null)
        {
            Vector3 horizontalVel = Vector3.ProjectOnPlane(_playerController.velocity, Vector3.up);
            speed = horizontalVel.magnitude;
        }
        else
        {
            // fallback: approximate from position delta
            Vector3 delta = _playerTransform.position - _previousPlayerPos;
            speed = new Vector3(delta.x, 0f, delta.z).magnitude / Mathf.Max(Time.deltaTime, 1e-6f);
            _previousPlayerPos = _playerTransform.position;
        }

        Vector3 bobOffset = Vector3.zero;
        if (_enableBobbing && speed > _movementThreshold)
        {
            float speedNormalized = Mathf.Clamp01(speed / Mathf.Max(0.0001f, _maxSpeedForBobbing));
            _bobTimer += Time.deltaTime * _bobFrequency * (0.5f + speedNormalized * 0.5f);
            float targetBobY = Mathf.Sin(_bobTimer) * _bobAmplitude * speedNormalized;
            _currentBobY = Mathf.SmoothDamp(_currentBobY, targetBobY, ref _bobVelocity, _bobSmoothTime);

            // apply bob along the camera's local up so it behaves correctly at any camera angle (top-down included)
            bobOffset = transform.up * _currentBobY;
        }
        else
        {
            // smoothly return bob to zero when stopping
            _currentBobY = Mathf.SmoothDamp(_currentBobY, 0f, ref _bobVelocity, _bobSmoothTime);
            bobOffset = transform.up * _currentBobY;
        }

        Vector3 targetPosition = _playerTransform.position + _offset + bobOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, 1f / _followSpeed);
    }
}