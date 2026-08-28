using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    [Header("Object")]
    public Transform player;
    public Transform home;

    [Header("UI")]
    public Slider progressBar;
    public Text distanceText;
    public Text progressText;

    private float startDistance;

    void Start()
    {
        // Hitung jarak awal Player ke Rumah
        startDistance = Vector2.Distance(
            player.position,
            home.position
        );

        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0f;
        }
    }

    void Update()
    {
        CalculateProgress();
    }

    void CalculateProgress()
    {
        // Distance
        float currentDistance = Vector2.Distance(
            player.position,
            home.position
        );

        // Progress ke rumah
        float progress = 1f -
            (currentDistance / startDistance);

        progress = Mathf.Clamp01(progress);

        // Update Progress Bar
        if (progressBar != null)
        {
            progressBar.value = progress;
        }

        // Update Distance
        if (distanceText != null)
        {
            distanceText.text =
                "Distance: " +
                Mathf.Round(currentDistance) +
                " m";
        }

        // Update Progress %
        if (progressText != null)
        {
            progressText.text =
                "Progress: " +
                Mathf.Round(progress * 100f) +
                "%";
        }
    }
}