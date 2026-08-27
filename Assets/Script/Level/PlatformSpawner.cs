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
    [SerializeField] private float minYOffset = 0f;
    [SerializeField] private float maxYOffset = 0f;

    private float timer;
    private float nextSpawnTime;

    private void Start()
    {
        SetNextSpawnTime();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            CreatePlatform();

            timer = 0f;
            SetNextSpawnTime();
        }
    }

    private void CreatePlatform()
    {
        if (floorPrefab == null)
        {
            Debug.LogError(
                $"[{gameObject.name}] Floor Prefab NULL! Instance ID: {GetInstanceID()}",
                gameObject
            );

            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError(
                $"[{gameObject.name}] Spawn Point NULL! Instance ID: {GetInstanceID()}",
                gameObject
            );

            return;
        }

        Vector3 spawnPosition = spawnPoint.position;

        spawnPosition.y += Random.Range(
            minYOffset,
            maxYOffset
        );

        GameObject newFloor = Instantiate(
            floorPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log(
            $"Spawn berhasil: {newFloor.name} di {spawnPosition}"
        );
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(
            minSpawnDelay,
            maxSpawnDelay
        );
    }
}