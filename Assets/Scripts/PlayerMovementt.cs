using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.GroundFitter;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerMovementt : MonoBehaviour
{
    // === MOVEMENT SETTINGS ===
    [Header("Movement Settings")]
    public FGroundFitter_Movement movement;
    public Rig rig;
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;

    // === INPUT SETTINGS ===
    [Header("Input Settings")]
    public KeyCode jumpKey = KeyCode.Space;

    // === GROUND DETECTION ===
    [Header("Ground Detection")]
    public LayerMask whatIsGround;
    public Transform raycast;

    // === WATER DETECTION ===
    [Header("Water Detection")]
    public LayerMask whatIsWater;
    public float waterCheckDistance = 0.2f;

    // === REFERENCES ===
    [Header("References")]
    public Transform orientation;
    public Animator animator;

    // Public properties for animation controller to access
    public bool IsGrounded;
    public bool OnWater;
    public bool IsHiding  { get; private set; }
    public bool IsJumping { get; private set; }
    public bool readyToJump = true;
    

    // === UNITY LIFECYCLE ===
    private void Start()
    {

        if (orientation == null)
        {
            Debug.LogError("PlayerMovement requires an Orientation Transform reference!");
            return;
        }
        readyToJump = true;
    }

    private void Update()
    {
        CheckGroundStatus();
        CheckWaterStatus();
        HandleInput();
        UpdateMovementState();
        Hide();
    }

    // === GROUND DETECTION ===
    private void CheckGroundStatus()
    {
        IsGrounded = Physics.Raycast(raycast.position, Vector3.down,
                                   0.2f, whatIsGround);
    }

    // === HIDING DETECTION ===
    public void Hide()
    {
        if (Input.GetKeyDown(KeyCode.H) && !IsHiding && !OnWater)
        {
            IsHiding = true;
            animator.SetBool("IsHiding", true);
            movement.BaseSpeed = 0f;
            rig.weight = 0f;
            movement.enabled = false; // Disable movement (WASD)
        }
        else if (Input.GetKeyUp(KeyCode.H) && IsHiding)
        {
            IsHiding = false;
            animator.SetBool("IsHiding", false);
            movement.BaseSpeed = 7.5f;
            rig.weight = 1f;
            movement.enabled = true; // Enable movement (WASD)
        }
    }

    // === WATER DETECTION (RAYCAST) ===
    private void CheckWaterStatus()
    {
        bool wasOnWater = OnWater;
        OnWater = Physics.Raycast(raycast.position, Vector3.down, waterCheckDistance, whatIsWater);

        if (OnWater && !wasOnWater)
        {
            movement.BaseSpeed = 15f;
            rig.weight = 0f;
            animator.SetBool("OnWater", true);
        }
        else if (!OnWater && wasOnWater)
        {
            movement.BaseSpeed = 7.5f;
            rig.weight = 1f;
            animator.SetBool("OnWater", false);
        }
    }

    // === INPUT HANDLING ===
    private void HandleInput()
    {
        // Handle jumping
        if (Input.GetKey(jumpKey) && readyToJump)
        {
            PerformJump();
        }
    }


    private void UpdateMovementState()
    {
        if (IsJumping && IsGrounded)
        {
            IsJumping = false;
        }
    }

    // === JUMPING SYSTEM ===
    private void PerformJump()
    {
        if (!readyToJump) return;
        animator.SetBool("Jump", true);
        readyToJump = false;
        IsJumping = true;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump()
    {
        animator.SetBool("Jump", false);
        readyToJump = true;
    }
}