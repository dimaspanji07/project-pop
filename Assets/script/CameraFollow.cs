using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform playerTarget;

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothSpeed = 5f;

    [Header("Camera Bounds")]
    public bool useBounds = true;
    public float minX = 0f;
    public float maxX = 50f;
    public float minY = 0f;
    public float maxY = 10f;

    void LateUpdate()
    {
        if (playerTarget == null) return;

        // Calculate target position based on offset
        Vector3 targetPosition = playerTarget.position + offset;

        // Clamp X and Y positions if bounds are enabled
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}