using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public float smoothSpeed = 5f;
    public float offsetZ = -10f;

    private float fixedY;

    private void Start()
    {
        // Kamera hanya mengikuti X player.
        // Posisi Y kamera tetap.
        fixedY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = new Vector3(
            target.position.x,
            fixedY,
            offsetZ
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}