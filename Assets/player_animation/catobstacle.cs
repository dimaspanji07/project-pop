using System.Collections;
using UnityEngine;

public class catobstacle : MonoBehaviour
{
    private Animator anim;
    private bool hasTriggered = false;

    [Header("Settings")]
    public float delayGameOver = 0.3f; // Jeda waktu (detik) agar animasi kaget selesai diputar

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // 1. Jalankan animasi kaget
            if (anim != null)
            {
                anim.SetBool("IsHit", true);
            }

            // 2. Jalankan delay sebelum Game Over
            StartCoroutine(WaitAndGameOver());
        }
    }

    private IEnumerator WaitAndGameOver()
    {
        // Menunggu selama delayGameOver detik
        yield return new WaitForSeconds(delayGameOver);

        // Memanggil Game Over setelah animasi kaget selesai
        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
    }
}