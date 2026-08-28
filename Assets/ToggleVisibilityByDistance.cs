using UnityEngine;

public class ToggleVisibilityByDistance : MonoBehaviour
{
    public Transform cameraTransform;
    public float hideDistance = 30.0f;
    
    private SpriteRenderer[] renderers;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        float distanceX = Mathf.Abs(transform.position.x - cameraTransform.position.x);

        // Jika jauh, sembunyikan gambarnya. Jika mendekat, tampilkan lagi.
        bool shouldShow = distanceX <= hideDistance;

        foreach (var rend in renderers)
        {
            if (rend.enabled != shouldShow)
                rend.enabled = shouldShow;
        }
    }
}