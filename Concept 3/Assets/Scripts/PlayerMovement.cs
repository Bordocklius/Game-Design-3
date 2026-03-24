using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CharacterController _charController;
    [SerializeField, Min(1f)] private float _movementSpeed = 5f;


    private Vector2 _movementInput;
    private float _verticalVelocity;


    [Space(10), Header("Visuals")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _visualRoot;

    private Vector3 _previousMousePos = Vector3.zero;

    private void Awake()
    {
        if(_charController == null)
            _charController = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        Vector3 move = new Vector3(_movementInput.x, 0f, _movementInput.y);
        HandleRotation(move);

        move = Vector3.ClampMagnitude(move, 1f); // avoid faster diagonal speed
        move *= _movementSpeed;


        // Simple gravity
        if (_charController.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -1f; // small downward force to keep grounded
        else
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;

        move.y = _verticalVelocity;

        _charController.Move(move * Time.deltaTime);
    }

    private void HandleRotation(Vector3 moveDirection)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 targetPos = hit.point;
            Vector3 origin = _visualRoot.position;
            targetPos.y = origin.y;

            Vector3 lookdirection = targetPos - origin;
            if(lookdirection.sqrMagnitude < 0.0001f)
                return;

            Quaternion lookRotation = Quaternion.LookRotation(lookdirection);
            _visualRoot.rotation = lookRotation;
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }


    public IEnumerator ApplySpeedBuff(float buffAmount, float duration)
    {
        _movementSpeed += buffAmount;
        float t = 0f;
        while(t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        _movementSpeed -= buffAmount;
    }
}
