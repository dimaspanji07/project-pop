using UnityEngine;

public class move : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private float moveInput;

    [Header("Orientation")]
    private bool isFacingRight = true;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip jumpSFX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Auto-assign AudioSource if unassigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Horizontal input (A/D or Left/Right arrows)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip character direction based on movement
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Jump using Space key (only when on ground / Y velocity near 0)
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            // Play jump sound effect
            if (audioSource != null && jumpSFX != null)
            {
                audioSource.PlayOneShot(jumpSFX);
            }
        }
    }

    void FixedUpdate()
    {
        // Move left and right
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
