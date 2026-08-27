using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // A / D untuk gerak kiri kanan
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // W atau SPACE langsung lompat
        if (
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.Space)
        )
        {
            Jump();
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.velocity.y
        );
    }

    private void Jump()
    {
        rb.velocity = new Vector2(
            rb.velocity.x,
            jumpForce
        );
    }

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        // Animasi run hanya jalan ketika A/D ditekan
        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            animator.speed = 1f;
        }
        else
        {
            animator.speed = 0f;
        }
    }
}