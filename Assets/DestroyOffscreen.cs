using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    [SerializeField] private float destroyX = -20f;

    private void Update()
    {
        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}