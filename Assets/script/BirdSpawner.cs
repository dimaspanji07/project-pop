using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [Header("Prefab & Reference")]
    public GameObject birdPrefab;
    [Tooltip("Tarik GameObject SpawnPoint (child Player/Kamera di sebelah kanan) ke sini")]
    public Transform spawnPoint; 

    [Header("Waktu Spawn Acak")]
    public float minSpawnDelay = 2.0f;
    public float maxSpawnDelay = 5.0f;

    [Header("Variasi Tinggi Acak dari Player")]
    public float minOffsetY = 0.5f; 
    public float maxOffsetY = 3.0f; 

    [Header("Despawn Settings")]
    [Tooltip("Margin batas luar kiri kamera sebelum burung di-destroy (misal: 0.1 agar burung menghilang tepat setelah keluar layar)")]
    public float offscreenMargin = 0.1f; 

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // INTEGRASI GAMEMANAGER: Cek kondisi game
            if (GameManager.instance != null && GameManager.instance.currentState != GameManager.GameState.Playing)
            {
                continue;
            }

            if (birdPrefab != null && spawnPoint != null)
            {
                float spawnX = spawnPoint.position.x;
                float randomOffsetY = Random.Range(minOffsetY, maxOffsetY);
                float spawnY = spawnPoint.position.y + randomOffsetY;

                Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
                
                // 1. Spawn burung
                GameObject spawnedBird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);

                // 2. Jalankan coroutine despawn berbasis batas kamera
                StartCoroutine(AutoDespawnRoutine(spawnedBird));
            }
        }
    }

    // Coroutine menghapus burung saat posisinya sudah berada di sebelah kiri batas kamera
    private IEnumerator AutoDespawnRoutine(GameObject bird)
    {
        while (bird != null)
        {
            if (mainCamera != null)
            {
                // Ubah posisi dunia burung ke koordinat Viewport kamera
                // Viewport Point: (0,0) adalah kiri-bawah, (1,1) adalah kanan-atas
                Vector3 viewportPos = mainCamera.WorldToViewportPoint(bird.transform.position);

                // Jika nilai X kurang dari (0 - margin), artinya burung sudah keluar di sebelah kiri layar
                if (viewportPos.x < -offscreenMargin)
                {
                    Destroy(bird);
                    yield break;
                }
            }

            yield return new WaitForSeconds(0.2f); // Cek setiap 0.2 detik untuk optimasi
        }
    }
}