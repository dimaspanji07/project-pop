using UnityEngine;

public class FloorMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private void Update()
    {
        transform.Translate(
            Vector3.left * moveSpeed * Time.deltaTime
        );
    }
}