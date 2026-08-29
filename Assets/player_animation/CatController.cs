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

    // Menyimpan lantai/platform yang sedang disentuh.
    private readonly HashSet<Collider2D> groundContacts = new();

    // Animator Parameters
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
        // Kalau sudah masuk void,
        // player tidak bisa dikontrol lagi.
        if (isVoidFalling)
        {
            horizontalInput = 0f;
            UpdateAnimator();
            return;
        }

        ReadMovementInput();
        HandleJump();
        UpdateFacingDirection();
        UpdateAnimator();
    }


    private void FixedUpdate()
    {
        if (isVoidFalling)
        {
            // Saat jatuh ke void hanya biarkan jatuh vertikal.
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


    // =====================================================
    // MOVEMENT
    // =====================================================

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


    // =====================================================
    // JUMP
    // =====================================================

    private void HandleJump()
    {
        bool jumpPressed =
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.W);

        if (!jumpPressed)
            return;

        // Harus menyentuh tanah.
        if (!isGrounded)
            return;

        // Tidak boleh double jump.
        if (jumpUsed)
            return;

        Jump();
    }


    private void Jump()
    {
        jumpUsed = true;
        isGrounded = false;

        groundContacts.Clear();

        // Reset Y agar tinggi jump konsisten.
        rb.velocity = new Vector2(
            rb.velocity.x,
            0f
        );

        rb.velocity = new Vector2(
            rb.velocity.x,
            jumpForce
        );
    }


    // =====================================================
    // ANIMATOR
    // =====================================================

    private void UpdateAnimator()
    {
        if (animator == null)
            return;

        bool isMoving =
            Mathf.Abs(horizontalInput) > 0.01f;

        /*
         * Jump animation:
         *
         * Selama player berada di udara NORMAL,
         * tetap pakai Cat_Jump.
         *
         * Jadi saat naik maupun turun setelah jump,
         * tetap dianggap Jump.
         *
         * Fall hanya muncul kalau masuk VoidZone.
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


    // =====================================================
    // GROUND DETECTION
    // =====================================================

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

        /*
         * Saat player sedang naik setelah jump,
         * jangan langsung dianggap grounded lagi.
         */
        if (jumpUsed && rb.velocity.y > 0.1f)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            /*
             * Normal Y positif berarti ada collider
             * di bawah player.
             *
             * Jadi menyentuh tembok dari samping
             * tidak dianggap Ground.
             */
            if (contact.normal.y <= 0.5f)
                continue;

            groundContacts.Add(
                collision.collider
            );

            isGrounded = true;

            // Sudah mendarat.
            // Jump tersedia kembali.
            if (rb.velocity.y <= 0.1f)
            {
                jumpUsed = false;
            }

            return;
        }
    }


    // =====================================================
    // VOID / FALL
    // =====================================================

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
        jumpUsed = true;

        horizontalInput = 0f;

        groundContacts.Clear();

        // Langsung pindah ke Fall Animation.
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

        Debug.Log("CAT FALL → Player masuk VoidZone");
    }


    // =====================================================
    // RESPAWN
    // =====================================================

    public void ResetPlayerState()
    {
        isVoidFalling = false;
        isGrounded = false;
        jumpUsed = false;

        horizontalInput = 0f;

        groundContacts.Clear();

        rb.velocity = Vector2.zero;

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