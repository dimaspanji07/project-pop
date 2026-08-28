using UnityEngine;

public class ParallaxLoop : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;

    [Header("Looping Bounds")]
    public float resetPositionX;
    public float startPositionX;

    private void Update()
    {
        // Move object to the left
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Reset position to startPositionX when reaching resetPositionX
        if (transform.position.x <= resetPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}