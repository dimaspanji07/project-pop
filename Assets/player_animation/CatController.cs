using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class CatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump & Physics")]
    [SerializeField] private float jumpForce = 8f;
    [Tooltip("Pengali gravitasi ekstra HANYA saat player sedang melayang turun/jatuh")]
    [SerializeField] private float fallMultiplier = 2.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;

    private bool isGrounded;
    private bool jumpUsed;
    private bool isVoidFalling;

    // Menyimpan collider tanah yang sedang disentuh.
    private readonly HashSet<Collider2D> groundContacts = new();

    // Animator parameter hash
    private static readonly int IsMovingHash =
        Animator.StringToHash("isMoving");

    private static readonly int IsJumpingHash =
        Animator.StringToHash("isJumping");

    private static readonly int IsFallingHash =
        Animator.StringToHash("isFalling");


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (isVoidFalling)
        {
            horizontalInput = 0f;
            UpdateAnimation();
            return;
        }

        ReadInput();
        HandleJump();
        ApplyCustomFallPhysics(); // Modifikasi: Mempercepat kecepatan jatuh
        UpdateAnimation();
        UpdateFacingDirection();
    }


    private void FixedUpdate()
    {
        // Saat jatuh ke void, player tidak bisa dikontrol lagi.
        if (isVoidFalling)
        {
            rb.velocity = new Vector2(
                0f,
                rb.velocity.y
            );

            return;
        }

        rb.velocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.velocity.y
        );
    }


    // =========================
    // FALL PHYSICS MODIFICATION
    // =========================

    private void ApplyCustomFallPhysics()
    {
        // Menambahkan ekstra gravitasi HANYA ketika kecepatan Y negatif (sedang turun/jatuh)
        if (rb.velocity.y < 0f)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
    }


    // =========================
    // MOVEMENT
    // =========================

    private void ReadInput()
    {
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
    }


    private void UpdateFacingDirection()
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


    // =========================
    // JUMP
    // =========================

    private void HandleJump()
    {
        bool jumpPressed =
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.W);

        if (!jumpPressed)
            return;

        // Tidak boleh lompat kalau sedang di udara.
        if (!isGrounded)
            return;

        // Mencegah double jump.
        if (jumpUsed)
            return;

        Jump();
    }


    private void Jump()
    {
        jumpUsed = true;
        isGrounded = false;

        groundContacts.Clear();

        rb.velocity = new Vector2(
            rb.velocity.x,
            jumpForce
        );
    }


    // =========================
    // ANIMATION
    // =========================

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        bool isMoving =
            Mathf.Abs(horizontalInput) > 0.01f;

        bool isJumping =
            !isGrounded &&
            !isVoidFalling;

        animator.SetBool(
            IsMovingHash,
            isMoving
        );

        animator.SetBool(
            IsJumpingHash,
            isJumping
        );

        animator.SetBool(
            IsFallingHash,
            isVoidFalling
        );
    }


    // =========================
    // GROUND DETECTION
    // =========================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetectGround(collision);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        DetectGround(collision);
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        groundContacts.Remove(
            collision.collider
        );

        isGrounded =
            groundContacts.Count > 0;
    }


    private void DetectGround(Collision2D collision)
    {
        if (isVoidFalling)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y <= 0.5f)
                continue;

            groundContacts.Add(
                collision.collider
            );

            isGrounded = true;
            jumpUsed = false;

            return;
        }
    }


    // =========================
    // VOID
    // =========================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        EnterVoid();
    }


    private void EnterVoid()
    {
        if (isVoidFalling)
            return;

        isVoidFalling = true;
        isGrounded = false;

        groundContacts.Clear();

        horizontalInput = 0f;

        animator.SetBool(
            IsMovingHash,
            false
        );

        animator.SetBool(
            IsJumpingHash,
            false
        );

        animator.SetBool(
            IsFallingHash,
            true
        );

        Debug.Log("Player masuk VOID → Fall Animation");
    }


    public void ResetPlayerState()
    {
        isVoidFalling = false;
        isGrounded = false;
        jumpUsed = false;

        horizontalInput = 0f;

        groundContacts.Clear();

        animator.SetBool(
            IsMovingHash,
            false
        );

        animator.SetBool(
            IsJumpingHash,
            false
        );

        animator.SetBool(
            IsFallingHash,
            false
        );
    }
}