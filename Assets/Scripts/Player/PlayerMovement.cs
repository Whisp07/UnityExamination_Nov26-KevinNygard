using System;
using UnityEngine;
using UnityEngine.InputSystem;
/////////////// INFORMATION ///////////////
// SETUP:
// Add the script to the object you want the player to control
// TIPS:
// - The Rigidbody2D component should (probably) have some constraints: Freeze Rotation Z
// - Adjust the CapsuleCollider2D to fit the object appropriately
// - Objects that should be used as platforms needs to have a collider (non- trigger)
// LIMITATIONS:
// This script enables very limited platformer movement, but with minimal setup required.
// The available movement consists of: a jump with a fixed height (based onjumpForce), and moving side to side (based on maxSpeed).
// It will NOT easily enable you to add animations or sound or similar effects.
// This script automatically adds a PlayerInput, Rigidbody2D and aCapsuleCollider2D component in the inspector.
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D),
    typeof(CapsuleCollider2D))]
public class LimitedPlatformerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    
    private Vector2 moveInput;
    
    private bool jumpInput;
    
    private Rigidbody2D rigidBody;
    private CapsuleCollider2D groundCheckCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        groundCheckCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
// Apply horizontal velocity from input
        if (moveInput.x != 0) rigidBody.linearVelocityX = moveInput.x * maxSpeed;
// Apply vertical velocity from jump
        if (!IsGrounded()) animator.SetBool("grounded", false);
        else animator.SetBool("grounded", true);
        
        if (rigidBody.linearVelocityX > 0.2 || rigidBody.linearVelocityX < -0.2) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);

        if (rigidBody.linearVelocityX > 0) spriteRenderer.flipX = false;
        else if (rigidBody.linearVelocityX < 0) spriteRenderer.flipX = true;

        if (jumpInput && IsGrounded())
        {
            rigidBody.linearVelocityY = jumpForce;
        }
        jumpInput = false; // only jump once for each press of the jump-button
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Slippery"))
        {
            rigidBody.linearVelocityX *= 0.95f;
        }
        
        if (other.gameObject.CompareTag("Enemy")) rigidBody.angularVelocity += 10;
    }

    private bool IsGrounded()
    {
// Is our groundCheckCollider touching the groundLayer? If so, return the value "true"
        if (groundCheckCollider.IsTouchingLayers(~0)) return true; // ~0 is referring to EVERY layer.
        else return false;
    }
    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }
    public void OnJump(InputValue input)
    {
        jumpInput = true;
    }
}