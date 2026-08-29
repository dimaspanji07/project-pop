using UnityEngine;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Jika Player jatuh menyentuh area Fall Zone
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                // Ini akan langsung memicu GameOver via HandleGameOver() di GameManager
                GameManager.instance.ChangeState(GameManager.GameState.DieByVoid);
            }
            else
            {
                Debug.LogError("GameManager instance tidak ditemukan!");
            }
        }
        // Hapus obstacle atau burung jika ikut jatuh ke bawah
        else if (other.CompareTag("Obstacle") || other.CompareTag("Bird"))
        {
            Destroy(other.gameObject);
        }
    }
}