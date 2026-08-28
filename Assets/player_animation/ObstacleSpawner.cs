using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject catPrefab;             // Prefab Kucing Kuning

    [Header("Spawn Timing (Delay Waktu)")]
    public float minSpawnDelay = 3.0f;        // Jeda minimal (detik)
    public float maxSpawnDelay = 6.0f;        // Jeda maksimal (detik)

    [Header("Spawn Distance Settings")]
    public Transform cameraTransform;        // Main Camera
    public float distanceFromCamera = 12.0f; // Jarak spawn di sebelah kanan luar layar kamera
    
    [Header("Spacing Settings (Mencegah Spawn Berdekatan)")]
    public float minSpawnGap = 8.0f;          // Jarak minimal antar kucing (unit)

    [Header("Roof Detection Settings")]
    public LayerMask roofLayer;              // Layer 'roof'
    public float catHeightOffset = 0.5f;     // Pengatur tinggi kaki kucing di atas genteng

    [Header("Slope & Edge Safety Settings")]
    [Tooltip("Maksimal toleransi kemiringan (0 = benar-benar rata)")]
    public float maxSlopeTolerance = 0.05f; 
    
    [Tooltip("Jarak aman dari tepi/jurang atap dan medan miring (ke kiri dan kanan)")]
    public float safeBufferDistance = 1.5f; 

    private float lastSpawnX = -999f;        // Menyimpan koordinat X spawn terakhir

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            // 1. Tunggu jeda waktu acak
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // INTEGRASI GAMEMANAGER: Hanya spawn jika game sedang aktif
            if (GameManager.instance != null && GameManager.instance.currentState != GameManager.GameState.Playing)
            {
                continue; // Lompati proses spawn jika game over / pause
            }

            if (cameraTransform != null)
            {
                float currentTargetX = cameraTransform.position.x + distanceFromCamera;

                // 2. Cek apakah jarak dari kucing sebelumnya sudah mencukupi
                if (currentTargetX - lastSpawnX >= minSpawnGap)
                {
                    // 3. Cek apakah area tersebut aman (datar, tidak miring, dan bukan di tepi atap)
                    if (IsAreaFlatAndSafe(currentTargetX))
                    {
                        Vector2 rayOrigin = new Vector2(currentTargetX, 10.0f);
                        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 20.0f, roofLayer);

                        if (hit.collider != null)
                        {
                            float spawnY = hit.point.y + catHeightOffset;
                            Vector3 spawnPosition = new Vector3(currentTargetX, spawnY, 0f);

                            if (catPrefab != null)
                            {
                                Instantiate(catPrefab, spawnPosition, Quaternion.identity);
                                lastSpawnX = currentTargetX;
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IsAreaFlatAndSafe(float targetX)
    {
        float[] checkPositions = new float[] 
        { 
            targetX, 
            targetX - safeBufferDistance, 
            targetX + safeBufferDistance 
        };

        foreach (float posX in checkPositions)
        {
            Vector2 rayOrigin = new Vector2(posX, 10.0f);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 20.0f, roofLayer);

            if (hit.collider == null) return false; 
            if (Mathf.Abs(hit.normal.x) > maxSlopeTolerance) return false; 
        }

        return true; 
    }
}