using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Sam Robichaud NSCC-Truro, Based on tutorial by (Comp - 3 Interactive)  * with modifications *

public class FirstPersonController_Sam : MonoBehaviour
{
    public bool canMove = true;

    public bool cursorLocked = true;
    private bool isRunning => canRun && Input.GetKey(runKey);
    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

    #region Settings

    [Header("Functional Settings")]
    [SerializeField] private bool canRun = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool canSlideOnSlopes = true;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool useFootsteps = true;

    [Header("Controls")]
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;

    [Header("Move Settings")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float slopeSpeeed = 12f;

    [Header("Look Settings")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 70.0f;
    [SerializeField, Range(-180, 1)] private float lowerLookLimit = -70.0f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float timeToCrouch = 0.15f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Headbob Settings")]
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.05f;
    [SerializeField] private float walkBobSpeed = 12f;
    [SerializeField] private float walkBobAmount = 0.065f;
    [SerializeField] private float runBobSpeed = 16f;
    [SerializeField] private float runBobAmount = 0.1f;
    private float defaultYPos = 0;
    private float timer;

    [Header("Zoom Settings")]
    [SerializeField] private float timeToZoom = 0.2f;
    [SerializeField] private float zoomFOV = 30f;
    private float defaultFOV;
    private Coroutine zoomRoutine;

    [Header("Footstep Settings")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float RunStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodFootstepClips = default;
    [SerializeField] private AudioClip[] metalFootstepClips = default;
    [SerializeField] private AudioClip[] grassFootstepClips = default;
    private float footstepTimer = 0f;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isRunning ? baseStepSpeed * RunStepMultiplier : baseStepSpeed;

    // Sliding Settings
    private Vector3 hitPointNormal;
    private bool isSliding
    {
        get
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 5.0f))
            {
                hitPointNormal = slopeHit.normal;

                //prevents the player from jumping while sliding
                if (Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit)
                {
                    canJump = false;
                }
                else
                {
                    canJump = true;
                }
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else { return false; }
        }
    }

    [Header("Interaction Settings")]
    [SerializeField] private Vector3 interactionRayPoint = new Vector3(0.5f, 0.5f, 0);
    [SerializeField] private float interactionDistance = 2.0f;
    [SerializeField] private LayerMask interactionLayer = 7;
    private Interactable currentInteractable;

    #endregion

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;        
    }

    private void Update()
    {
        if (cursorLocked == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (cursorLocked == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        if (canMove)
        {
            HandleMovementInput();
            HandleMouseLook(); // look into moving into Lateupdate if motion is jittery

            if (canJump)        { HandleJump();                                         }
            if (canCrouch)      { HandleCrouch();                                       }
            if (canUseHeadbob)  { HandleHeadBob();                                      }
            if (canZoom)        { HandleZoom();                                         }
            if (canInteract)    { HandleInteractionCheck(); HandleInteractionInput();   }
            if (useFootsteps)   { HandleFootsteps();                                    }

            ApplyFinalMovement();
        }
    }

    private void LateUpdate()
    {

    }

    private void HandleMovementInput()
    {
        // Read inputs
        currentInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxis("Horizontal"));

        // normalizes input when 2 directions are pressed at the same time
        // TODO; find a more elegant solution to normalize, this is a bit of a hack method to normalize it estimates and is not 100% accurate.
        currentInput *= (currentInput.x != 0.0f && currentInput.y != 0.0f) ? 0.7071f : 1.0f;

        // Sets the required speed multiplier
        currentInput *= (isCrouching ? crouchSpeed : isRunning ? runSpeed : walkSpeed);

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        // Rotate camera up/down
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, lowerLookLimit, upperLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Rotate player left/right
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

    }

    private void HandleJump()
    {
        if (shouldJump)
        {
            moveDirection.y = jumpForce;
        }
    }

    private void HandleCrouch()
    {
        if (shouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void HandleHeadBob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isRunning ? runBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isRunning ? runBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }

        if (Input.GetKeyUp(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 7 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID() ) )
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                {
                    currentInteractable.OnFocus();
                }
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null; 
        }
    }

    private void HandleInteractionInput()
    {
        // TODO: Research TAG vs Layermask, at the moment it seems like using a tag as verificatin is going to work better


        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {            
            currentInteractable.OnInteract();
        }
    }

    private void HandleFootsteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;
              

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                

                switch (hit.collider.tag)
                {
                    case "Footsteps/Wood":
                        footstepAudioSource.PlayOneShot(woodFootstepClips[Random.Range(0, woodFootstepClips.Length - 1)]);
                        break;
                    case "Footsteps/Metal":
                        footstepAudioSource.PlayOneShot(metalFootstepClips[Random.Range(0, metalFootstepClips.Length - 1)]);
                        break;
                    case "Footsteps/Grass":
                        footstepAudioSource.PlayOneShot(grassFootstepClips[Random.Range(0, grassFootstepClips.Length - 1)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(metalFootstepClips[Random.Range(0, metalFootstepClips.Length - 1)]);
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;

        }

    }

    private void ApplyFinalMovement()
    {
        // Apply gravity if the character controller is not grounded
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (characterController.velocity.y < - 1 && characterController.isGrounded)
            moveDirection.y = 0;


        // sliding
        if (canSlideOnSlopes && isSliding)
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeeed;
        }

        // applies movement based on all inputs
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1.0f))
        { yield break; }
        
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView; // capture reference to current FOV
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }






}
