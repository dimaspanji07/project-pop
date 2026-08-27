using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform")]
    [SerializeField] private GameObject floorPrefab;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float minSpawnDelay = 3f;
    [SerializeField] private float maxSpawnDelay = 5f;

    [Header("Random Height")]
    [SerializeField] private float minYOffset = -0.5f;
    [SerializeField] private float maxYOffset = 0.5f;

    private float timer;
    private float nextSpawnTime;

    private void Start()
    {
        ResetSpawnTimer();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < nextSpawnTime)
            return;

        CreatePlatform();

        timer = 0f;
        ResetSpawnTimer();
    }

    private void CreatePlatform()
    {
        if (floorPrefab == null)
        {
            Debug.LogError("PlatformSpawner: Floor Prefab belum diisi.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("PlatformSpawner: Spawn Point belum diisi.");
            return;
        }

        Vector3 spawnPosition = spawnPoint.position;

        spawnPosition.y += Random.Range(
            minYOffset,
            maxYOffset
        );

        Instantiate(
            floorPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    private void ResetSpawnTimer()
    {
        nextSpawnTime = Random.Range(
            minSpawnDelay,
            maxSpawnDelay
        );
    }
}