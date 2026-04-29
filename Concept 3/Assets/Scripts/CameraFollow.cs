using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField, Min(0f)] private float _followSpeed = 5f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, -3f);


    [Space(10), Header("Leading")]
    [SerializeField] private float _leadDistance = 2.5f;
    [SerializeField] private float _leadSmoothTime = 0.15f;

    private Vector3 _localOffset;
    private Vector3 _currentLead;
    private Vector3 _leadVelocity;

    [Space(10), Header("Bobbing")]
    [SerializeField] private bool _enableBobbing;
    [SerializeField] private float _bobAmplitude = 0.05f;
    [SerializeField] private float _bobFrequency = 6f;
    [SerializeField] private float _bobSmoothTime = 0.08f;
    [SerializeField] private float _movementThreshold = 0.1f;
    [SerializeField] private float _maxSpeedForBobbing = 6f;

    [Space(10), Header("Mouse Lookahead")]
    [SerializeField] private bool _enableMouseLookAhead;
    [SerializeField] private float _mouseLookDistance;
    [SerializeField] private float _mouseSmoothTime;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private float _mouseDeadzoneRadius = 100f;

    private Vector3 _currentMouseLookOffset;
    private Vector3 _mouseLookVelocity;

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

        _localOffset = _playerTransform.InverseTransformDirection(transform.position - _playerTransform.position);
        //_offset = transform.position - _playerTransform.position;   

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

        float speed;
        Vector3 horizontalVel;
        (speed, horizontalVel) = CalculateSpeed();
        Vector3 bobOffset = CalculateBobOffset(speed);
        Vector3 desiredLead = CalculateMovementLead(speed, horizontalVel);
        Vector3 mouseOffset = CalculateMouseLookAhead();

        _currentLead = Vector3.SmoothDamp(_currentLead, desiredLead, ref _leadVelocity, _leadSmoothTime);

        Vector3 rotatedOffset = _playerTransform.TransformDirection(_localOffset);
        Vector3 targetPosition = _playerTransform.position + rotatedOffset + _currentLead + bobOffset + mouseOffset;

        //Vector3 targetPosition = _playerTransform.position + _offset + bobOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, 1f / _followSpeed);
    }

    private (float, Vector3) CalculateSpeed()
    {
        // compute horizontal movement speed (prefer CharacterController.velocity)
        float speed = 0f;
        Vector3 horizontalVel = Vector3.zero;
        if (_playerController != null)
        {
            horizontalVel = Vector3.ProjectOnPlane(_playerController.velocity, Vector3.up);
            speed = horizontalVel.magnitude;
        }
        else
        {
            // fallback: approximate from position delta
            Vector3 delta = _playerTransform.position - _previousPlayerPos;
            speed = new Vector3(delta.x, 0f, delta.z).magnitude / Mathf.Max(Time.deltaTime, 1e-6f);
            _previousPlayerPos = _playerTransform.position;
        }

        return (speed, horizontalVel);
    }

    private Vector3 CalculateBobOffset(float speed)
    {
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
        return bobOffset;
    }

    private Vector3 CalculateMovementLead(float speed, Vector3 horizontalVel)
    {
        Vector3 desiredLead = Vector3.zero;
        if (speed > _movementThreshold)
        {
            Vector3 movementDir = horizontalVel.magnitude > 0.001f ? horizontalVel.normalized : Vector3.zero;
            float speedNormalized = Mathf.Clamp01(speed / Mathf.Max(0.0001f, _maxSpeedForBobbing));
            float leadMag = Mathf.Lerp(0f, _leadDistance, speedNormalized);
            desiredLead = movementDir * leadMag;
        }
        return desiredLead;
    }

    private Vector3 CalculateMouseLookAhead()
    {
        Vector3 desiredMouseOffset = Vector3.zero;

        if (_enableMouseLookAhead)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // Project player position to screen space
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(_playerTransform.position);
            Vector2 playerScreenPos2D = new Vector2(playerScreenPos.x, playerScreenPos.y);

            // Calculate distance from player to mouse position in screen space
            float distanceFromPlayer = Vector2.Distance(mousePos, playerScreenPos2D);

            if (distanceFromPlayer > _mouseDeadzoneRadius)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
                Vector3 mouseWorldPos = mouseRay.origin + mouseRay.direction * _raycastDistance;

                // Check if there's a hit point closer than the default raycast distance
                if (Physics.Raycast(mouseRay, out RaycastHit hit, _raycastDistance))
                {
                    mouseWorldPos = hit.point;
                }

                // Calculate direction from player to mouse point
                Vector3 dirToMouse = (mouseWorldPos - _playerTransform.position).normalized;
                desiredMouseOffset = dirToMouse * _mouseLookDistance;
            }            
        }

        _currentMouseLookOffset = Vector3.SmoothDamp(_currentMouseLookOffset, desiredMouseOffset, ref _mouseLookVelocity, _mouseSmoothTime);
        return _currentMouseLookOffset;
    }
}