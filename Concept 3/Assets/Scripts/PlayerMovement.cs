using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Space(10), Header("InputActionReferences")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _mousePosAction;
 
    [Space(10), Header("Settings")]
    [SerializeField] private CharacterController _charController;
    [SerializeField, Min(1f)] private float _movementSpeed = 5f;


    private Vector2 _movementInput;
    private float _verticalVelocity;
    private bool _isHoldingMouse;
    private Vector3 _targetPosition;
    private bool _hasTarget;


    [Space(10), Header("Visuals")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _visualRoot;

    private Vector3 _previousMousePos = Vector3.zero;    

    private void Awake()
    {
        if(_charController == null)
            _charController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _moveAction.action.performed += MoveAction_Performed;
    }    

    private void OnDisable()
    {
        _moveAction.action.performed -= MoveAction_Performed;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;
        // Horizontal movement
        if(_hasTarget)
        {
            move = HandleMovementInput();
        }

        // Horizontal movement
        //Vector3 move = new Vector3(_movementInput.x, 0f, _movementInput.y);
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

    private Vector3 HandleMovementInput()
    {
        Vector3 directionToTarget = _targetPosition - transform.position;
        directionToTarget.y = 0f;
        
        // Check if we've reached the target
        if (directionToTarget.sqrMagnitude < 0.1f)
        {
            _hasTarget = false;
            return Vector3.zero;
        }
        
        return directionToTarget.normalized;
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

    private void MoveAction_Performed(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = _mousePosAction.action.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask, QueryTriggerInteraction.Ignore))
        {
            _targetPosition = hit.point;
            _hasTarget = true;
        }
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
