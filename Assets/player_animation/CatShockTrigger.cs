using UnityEngine;

public class CatShockTrigger : MonoBehaviour
{
    [SerializeField] private Animator orenAnimator;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        orenAnimator.SetTrigger("Kaget");

        Debug.Log("GAME OVER");
    }
}