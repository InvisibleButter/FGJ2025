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

    public LayerMask layerMask;
    
    private bool _isInWatcherMode = true;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        currentYaw = cameraTransform.eulerAngles.y;
        targetYaw = currentYaw;

        MovementAllowed = true;
        
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    private void Start()
    {
        if(ServiceLocator.Instance.HasService<ShroomGridService>())
        {
            ServiceLocator.Instance.GetService<ShroomGridService>().UpDateAllCells(_isInWatcherMode);
        }
        else
        {
            ServiceLocator.Instance.OnAllInitilized += () => ServiceLocator.Instance.GetService<ShroomGridService>().UpDateAllCells(_isInWatcherMode);
        }
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

    public void OnDebugCameraSwap(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        _isInWatcherMode=!_isInWatcherMode;
        ServiceLocator.Instance.GetService<ShroomGridService>().UpDateAllCells(_isInWatcherMode);
    }

    private void Raycast()
    {
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out var hit, 3))
        {
            _currentHittedEntity = hit.transform.GetComponent<GridEntity>();
        }
    }
    
    private float _flySpeed = 80f;

    public void FlyToPoint(Vector3 target)
    {
        StartCoroutine(FlyRoutine(target));
    }

    private IEnumerator FlyRoutine(Vector3 target)
    {
        var distance = Vector3.Distance(transform.position, target);
        while (distance > 1.5f)
        {
            distance = Vector3.Distance(transform.position, target);
            transform.position = Vector3.MoveTowards(transform.position, target +new Vector3(0, 1.5f, 0), _flySpeed * Time.deltaTime);
            yield return null;
        }

        MovementAllowed = true;
    }
}
