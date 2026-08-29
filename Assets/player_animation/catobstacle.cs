using System.Collections;
using UnityEngine;

public class catobstacle : MonoBehaviour
{
    private Animator anim;
    private bool hasTriggered = false;

    [Header("Settings")]
    public float delayGameOver = 0.5f; // Jeda animasi

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Pengecekan aman tag Player
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // 1. Jalankan animasi kaget
            if (anim != null)
            {
                anim.SetBool("IsHit", true);
            }

            // 2. Hentikan pergerakan player agar tidak tetap berjalan saat kaget
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
            }

            // 3. Jalankan delay
            StartCoroutine(WaitAndGameOver());
        }
    }

    private IEnumerator WaitAndGameOver()
    {
        // Menggunakan Realtime agar delay tetap jalan jika Time.timeScale = 0
        yield return new WaitForSecondsRealtime(delayGameOver);

        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
    }
}