using UnityEngine;

public class FishCollectible : MonoBehaviour
{
    [Header("Fish")]
    public float staminaRestore = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null)
        {
            // Tambahkan stamina
            player.RestoreStamina(staminaRestore);

            Debug.Log(
                "Fish diambil! Stamina +" +
                staminaRestore
            );

            // Hapus fish
            Destroy(gameObject);
        }
    }
}