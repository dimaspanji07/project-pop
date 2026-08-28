using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject losePanel;
    public GameObject winPanel;

    private bool gameFinished = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (losePanel != null)
            losePanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void LoseGame()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        Debug.Log("GAME OVER!");

        if (losePanel != null)
            losePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        Debug.Log("YOU WIN!");

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}