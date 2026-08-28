using UnityEngine;

public class FishCollectible : MonoBehaviour
{
    [Header("Fish")]
    [SerializeField] private float staminaRestore = 20f;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (other.TryGetComponent<PlayerController>(out var player))
        {
            player.RestoreStamina(staminaRestore);
            Debug.Log($"Fish diambil! Stamina +{staminaRestore}");
            Destroy(gameObject);
        }
    }
}