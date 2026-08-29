using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah objek ber-tag Obstacle ATAU Bird
        if (other.CompareTag("Obstacle") || other.CompareTag("bird"))
        {
            Destroy(other.gameObject);
        }
    }
}