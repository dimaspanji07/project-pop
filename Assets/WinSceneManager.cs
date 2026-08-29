using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Durasi animasi berputar dalam detik")]
    public float animationDuration = 6f;

    [Tooltip("Nama Scene setelah animasi selesai")]
    public string nextSceneName = "MainMenu";

    private IEnumerator Start()
    {
        // Tunggu selama 6 detik
        yield return new WaitForSeconds(animationDuration);

        // Pindah ke scene berikutnya
        SceneManager.LoadScene(nextSceneName);
    }
}