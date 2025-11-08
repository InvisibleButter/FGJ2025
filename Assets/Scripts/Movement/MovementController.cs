using System;
using System.Collections;
using Scripts.Grid;
using Scripts.Shrooms;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float rotationThreshold = 0.05f; // Deadzone for smoother rotation

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float targetYaw;  // Smoothed target yaw rotation
    private float currentYaw;

    public GridEntity CurrentHittedEntity => _currentHittedEntity;
    private GridEntity _currentHittedEntity;
    public Vector3 CameraForward => cameraTransform.forward;

    public bool MovementAllowed;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        currentYaw = cameraTransform.eulerAngles.y;
        targetYaw = currentYaw;

        MovementAllowed = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCameraRotation();
        HandleMovement();
    }

    private void HandleCameraRotation()
    {
        // Apply threshold to prevent tiny mouse movements
        if (Mathf.Abs(lookInput.x) > rotationThreshold)
        {
            targetYaw += lookInput.x * mouseSensitivity;
        }

        // Smoothly rotate toward target yaw
        currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, Time.deltaTime * rotationSpeed);
        cameraTransform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

    private void HandleMovement()
    {
        // Move relative to camera direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;

        Vector3 move = (right * moveInput.x + forward * moveInput.y).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Rotate player smoothly toward movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    // === Input System ===
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!MovementAllowed)
        {
            return;
        }
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!MovementAllowed)
        {
            return;
        } 
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        MovementAllowed = false;
        Raycast();
        ServiceLocator.Instance.GetService<ShroomAbilityService>().OnAbilityClicked(ShroomAbilityType.Walker, this);
    }
    
    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        
        Raycast();
        ServiceLocator.Instance.GetService<ShroomAbilityService>().OnAbilityClicked(ShroomAbilityType.Watcher, this);
    }
    
    public void OnAbility3(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        
        ServiceLocator.Instance.GetService<ShroomAbilityService>().OnAbilityClicked(ShroomAbilityType.Builder, this);
    }

    private void Raycast()
    {
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out var hit, 3))
        {
            _currentHittedEntity = hit.transform.GetComponent<GridEntity>();
        }
    }
    
    [SerializeField] private float flySpeed = 10f;

    public void FlyToPoint(Vector3 target)
    {
        StartCoroutine(RocketFlyRoutine(target + new Vector3(0, 1.5f, 0)));
    }

    private IEnumerator FlyRoutine(Vector3 target)
    {
        var distance = Vector3.Distance(transform.position, target);
        while (distance > 1.5f)
        {
            distance = Vector3.Distance(transform.position, target);
            transform.position = Vector3.MoveTowards(transform.position, target +new Vector3(0, 1.5f, 0), flySpeed * Time.deltaTime);
            yield return null;
        }

        MovementAllowed = true;
    }
   
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 40f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float stopDistance = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private IEnumerator RocketFlyRoutine(Vector3 target)
    {
        Vector3 startPos = transform.position;
        Vector3 direction = (target - startPos).normalized;
        float totalDistance = Vector3.Distance(startPos, target);

        while (true)
        {
            // How far we've traveled in the flight direction
            float traveled = Vector3.Dot(transform.position - startPos, direction);

            // Stop if we've reached or passed the target distance
            if (traveled >= totalDistance)
                break;

            float remaining = Mathf.Max(totalDistance - traveled, 0f);

            // Compute target speed (slow down near the end)
            float targetSpeed = maxSpeed;
            if (remaining < (maxSpeed * maxSpeed) / (2f * deceleration))
                targetSpeed = Mathf.Sqrt(2f * deceleration * remaining);

            // Accelerate/decelerate smoothly
            float currentSpeed = velocity.magnitude;
            float speedDiff = targetSpeed - currentSpeed;
            float accel = (speedDiff > 0 ? acceleration : deceleration) * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel);

            // Update velocity & move
            velocity = direction * currentSpeed;
            transform.position += velocity * Time.deltaTime;

            yield return null;
        }

        // Snap to exact target and stop
        transform.position = target;
        velocity = Vector3.zero;
        MovementAllowed = true;
    }
}
