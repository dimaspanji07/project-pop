using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;

    private bool isGrounded;
    private bool jumpConsumed;

    // Menyimpan collider lantai yang sedang disentuh.
    private readonly HashSet<Collider2D> groundContacts = new();

    private static readonly int IsJumpingHash =
        Animator.StringToHash("isJumping");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ReadMovementInput();
        HandleJump();
        HandleAnimation();
        HandleFacingDirection();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.velocity.y
        );
    }

    private void ReadMovementInput()
    {
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
    }

    private void HandleJump()
    {
        bool jumpPressed =
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.W);

        if (!jumpPressed)
            return;

        // Tidak boleh double jump.
        if (!isGrounded || jumpConsumed)
            return;

        Jump();
    }

    private void Jump()
    {
        jumpConsumed = true;
        isGrounded = false;

        // Jangan anggap masih grounded pada frame jump.
        groundContacts.Clear();

        rb.velocity = new Vector2(
            rb.velocity.x,
            jumpForce
        );
    }

    private void HandleAnimation()
{
    if (animator == null)
        return;

    bool isMoving = Mathf.Abs(horizontalInput) > 0.01f;

    animator.SetBool(
        "isMoving",
        isMoving
    );

    animator.SetBool(
        "isJumping",
        !isGrounded
    );
    }

    private void HandleFacingDirection()
    {
        if (spriteRenderer == null)
            return;

        if (horizontalInput > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGroundCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckGroundCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundContacts.Remove(collision.collider);

        isGrounded = groundContacts.Count > 0;
    }

    private void CheckGroundCollision(Collision2D collision)
    {
        // Saat masih bergerak naik setelah jump,
        // jangan langsung dianggap grounded lagi.
        if (jumpConsumed && rb.velocity.y > 0.1f)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Normal mengarah ke atas berarti collider
            // berada di bawah kaki player.
            if (contact.normal.y > 0.5f)
            {
                groundContacts.Add(collision.collider);

                isGrounded = true;

                // Sudah mendarat -> boleh jump lagi.
                if (rb.velocity.y <= 0.1f)
                {
                    jumpConsumed = false;
                }

                return;
            }
        }
    }
}