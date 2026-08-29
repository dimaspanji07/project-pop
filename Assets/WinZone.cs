using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    [Tooltip("Nama Scene tempat animasi diputar")]
    public string winSceneName = "WinScene";

    private bool hasWon = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasWon)
        {
            hasWon = true;

            // Pindah langsung ke Scene Animasi saat menyentuh WinState
            Time.timeScale = 1f;
            SceneManager.LoadScene(winSceneName);
        }
    }
}