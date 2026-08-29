using UnityEngine;

public class BirdObstacle : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 8f; // Hapus otomatis setelah 8 detik agar RAM tidak penuh

    void Start()
    {
        // Hapus burung otomatis jika sudah terbang jauh
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Bergerak lurus ke kiri
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika menabrak Player -> Game Over
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
            else
            {
                Debug.Log("Game Over!");
                Time.timeScale = 0f;
            }
        }
    }
}