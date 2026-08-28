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

    private void Start()
    {
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
                Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}