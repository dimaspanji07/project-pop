using UnityEngine;

/// <summary>
/// PlatformMover
/// Ditempel otomatis oleh script spawner ke setiap platform yang baru muncul.
/// Membuat platform bergerak terus ke kiri dengan kecepatan tetap,
/// lalu menghapus dirinya sendiri saat sudah keluar dari sisi kiri layar.
/// </summary>
public class PlatformMover : MonoBehaviour
{
    [Tooltip("Kecepatan geser ke kiri (unit per detik)")]
    public float speed = 3f;

    [Tooltip("Posisi X di mana platform dianggap sudah keluar layar dan akan dihapus")]
    public float despawnX = -15f;

    private Rigidbody2D rb;

    void Awake()
    {
        // Kalau platform punya Rigidbody2D (disarankan agar Player yang berdiri
        // di atasnya ikut kebawa gerak), pakai itu. Kalau tidak ada, gerak lewat Transform biasa.
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 movement = Vector2.left * speed * Time.fixedDeltaTime;

        if (rb != null)
        {
            // MovePosition lebih akurat untuk platform yang membawa Player di atasnya
            rb.MovePosition(rb.position + movement);
        }
        else
        {
            transform.Translate(movement);
        }

        if (transform.position.x < despawnX)
        {
            Destroy(gameObject);
        }
    }
}