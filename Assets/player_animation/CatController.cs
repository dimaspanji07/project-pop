using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
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
        UpdateAnimation();
        UpdateFacingDirection();
    }


    private void FixedUpdate()
    {
        // Saat jatuh ke void,
        // player tidak bisa dikontrol lagi.
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

        /*
         * PENTING:
         *
         * Jump tetap TRUE selama player berada
         * di udara NORMAL.
         *
         * Jadi ketika selesai naik lalu turun,
         * animasi masih Cat_Jump.
         *
         * Cat_Fall hanya dipakai kalau
         * isVoidFalling == true.
         */

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
        // Kalau sudah masuk void,
        // jangan dianggap grounded lagi.
        if (isVoidFalling)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            /*
             * normal.y > 0.5
             * berarti collider berada
             * di bawah Player.
             */

            if (contact.normal.y <= 0.5f)
                continue;

            groundContacts.Add(
                collision.collider
            );

            isGrounded = true;

            // Sudah mendarat.
            // Jump boleh digunakan lagi.
            jumpUsed = false;

            return;
        }
    }


    // =========================
    // VOID
    // =========================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Void"))
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

        // Langsung paksa parameter animator.
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


    // Bisa dipakai nanti ketika respawn.
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