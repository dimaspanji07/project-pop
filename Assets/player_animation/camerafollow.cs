using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Drag object Player/Kucing ke sini
    public float smoothSpeed = 5f; // Kecepatan pergerakan kamera
    public float offsetZ = -10f;  // Jarak kedalaman kamera 2D
    public float deadZoneX = 2f;  // Batas toleransi jarak sebelum kamera mulai bergerak

    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Hitung jarak horizontal antara kamera dan pemain
        float deltaX = target.position.x - transform.position.x;

        // Kamera hanya bergerak jika pemain bergerak melewati batas deadZoneX
        if (Mathf.Abs(deltaX) > deadZoneX)
        {
            // Tentukan target posisi X baru (mempertahankan jarak deadZone)
            float targetX = target.position.x - (Mathf.Sign(deltaX) * deadZoneX);
            Vector3 targetPosition = new Vector3(targetX, fixedY, offsetZ);

            // Gerakan halus ke posisi target
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}