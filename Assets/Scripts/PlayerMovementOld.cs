using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles player movement mechanics: walking, running, jumping, and ground detection.
/// Animation is handled by a separate PlayerAnimationController script.
/// </summary>
public class PlayerMovementOld : MonoBehaviour
{
    // === MOVEMENT SETTINGS ===
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float groundDrag = 5f;
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;

    // === INPUT SETTINGS ===
    [Header("Input Settings")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    // === GROUND DETECTION ===
    [Header("Ground Detection")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public Transform raycast;

    // === REFERENCES ===
    [Header("References")]
    public Transform orientation;
    public Animator animator;

    // Public properties for animation controller to access
    public bool IsGrounded;
    public bool OnWater;
    public bool IsJumping { get; private set; }
    public float CurrentSpeed { get; private set; }
    public bool IsSprintPressed { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public bool HasMovementInput { get; private set; }

    // === PRIVATE VARIABLES ===
    private float horizontalInput;
    private float verticalInput;
    public bool readyToJump = true;
    private Rigidbody rb;

    // === UNITY LIFECYCLE ===
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Safety checks
        if (rb == null)
        {
            Debug.LogError("PlayerMovement requires a Rigidbody component!");
            return;
        }

        if (orientation == null)
        {
            Debug.LogError("PlayerMovement requires an Orientation Transform reference!");
            return;
        }

        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        CheckGroundStatus();
        HandleInput();
        UpdateMovementState();
        // ApplyDrag();
        // SetAnimatorVariables();
    }

    // private void FixedUpdate()
    // {
    //     ApplyMovement();
    // }

    // === GROUND DETECTION ===
    private void CheckGroundStatus()
    {
        IsGrounded = Physics.Raycast(raycast.position, Vector3.down,
                                   0.2f, whatIsGround);
    }

    // === INPUT HANDLING ===
    private void HandleInput()
    {
        // // Get movement input
        // horizontalInput = Input.GetAxisRaw("Horizontal");
        // verticalInput = Input.GetAxisRaw("Vertical");

        // // Update input states
        // HasMovementInput = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
        // IsSprintPressed = Input.GetKey(sprintKey);

        // Handle jumping
        if (Input.GetKey(jumpKey) && readyToJump)
        {
            PerformJump();
        }
    }

    // === MOVEMENT SYSTEM ===
// private void ApplyMovement()
// {
//     MoveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
//     float currentMoveSpeed = (IsSprintPressed && IsGrounded) ? moveSpeed * 2f : moveSpeed;

//     if (IsGrounded)
//     {
//         rb.velocity = new Vector3(
//             MoveDirection.normalized.x * currentMoveSpeed,
//             rb.velocity.y,
//             MoveDirection.normalized.z * currentMoveSpeed
//         );
//     }
//     else
//     {
//         rb.AddForce(MoveDirection.normalized * currentMoveSpeed * 10f * airMultiplier, ForceMode.Force);
//     }
// }


    private void UpdateMovementState()
    {
        // Calculate current speed
        // Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        // CurrentSpeed = flatVelocity.magnitude;

        // Update jumping state
        if (IsJumping && IsGrounded)
        {
            IsJumping = false;
        }

        // // Calculate speed limit (double if sprinting)
        // float speedLimit = IsSprintPressed ? moveSpeed * 2f : moveSpeed;

        // // Limit speed if needed
        // if (flatVelocity.magnitude > speedLimit)
        // {
        //     Vector3 limitedVelocity = flatVelocity.normalized * speedLimit;
        //     rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        // }
    }

    // private void ApplyDrag()
    // {
    //     rb.drag = IsGrounded ? groundDrag : 0;
    // }

    // === JUMPING SYSTEM ===
    private void PerformJump()
    {
        animator.SetBool("Jump", true);
        readyToJump = false;
        IsJumping = true;

        // Reset vertical velocity for consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply jump force
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // Reset jump availability after cooldown
        Invoke(nameof(ResetJump), jumpCooldown);
    }
    // private void SetAnimatorVariables()
    // {
    //     if (!IsMoving())
    //     {
    //         animator.SetFloat("Speed", 0f);
    //     }
    //     else if (CurrentSpeed <= moveSpeed)
    //     {
    //         animator.SetFloat("Speed", 1f);
    //     }
    //     else if (CurrentSpeed == moveSpeed * 2f)
    //     {
    //         animator.SetFloat("Speed", 2f);
    //     }
    // }

    private void ResetJump()
    {
        animator.SetBool("Jump", false);
        readyToJump = true;
    }

    // === PUBLIC UTILITY METHODS ===
    public Vector2 GetMovementInput()
    {
        return new Vector2(horizontalInput, verticalInput);
    }

    public Vector3 GetNormalizedMoveDirection()
    {
        return MoveDirection.normalized;
    }

    public bool IsMoving()
    {
        return CurrentSpeed > 0.1f;
    }

    // === DEBUG TOOLS ===
    private void OnDrawGizmosSelected()
    {
        // Ground check ray
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f));

        // Movement direction
        if (Application.isPlaying && MoveDirection != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, MoveDirection * 2f);
        }

        // Velocity vector
        if (Application.isPlaying && rb != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, flatVel);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            moveSpeed = 15f;
            OnWater = true;
            animator.SetBool("OnWater", true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            moveSpeed = 7.5f;
            OnWater = false;
            animator.SetBool("OnWater", false);
        }
    }
}