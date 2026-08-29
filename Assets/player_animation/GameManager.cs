using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Enum daftar kondisi game
    public enum GameState
    {
        Playing,
        DieByBird,
        DieByCat,
        DieByVoid,
        GameWin,
        Paused
    }

    [Header("Kondisi Saat Ini")]
    public GameState currentState;

    [Header("UI Reference")]
    public GameObject pausePanel;
    public GameObject gameWinPanel;

    [Header("Scene Settings")]
    [Tooltip("Nama Scene Main Menu Anda")]
    public string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ChangeState(GameState.Playing);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (currentState == GameState.Playing) ChangeState(GameState.Paused);
            else if (currentState == GameState.Paused) ChangeState(GameState.Playing);
        }
    }

    // Fungsi utama mengubah state game
    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                SetPanelsActive(false, false);
                break;

            case GameState.DieByBird:
                Debug.Log("GAME OVER: Terkena Burung!");
                RestartGame();
                break;

            case GameState.DieByCat:
                Debug.Log("GAME OVER: Terkena Kucing Kuning!");
                RestartGame();
                break;

            case GameState.DieByVoid:
                Debug.Log("GAME OVER: Jatuh ke Jurang / Void!");
                RestartGame();
                break;

            case GameState.GameWin:
                Debug.Log("YOU WIN!");
                Time.timeScale = 1f; 
                SetPanelsActive(false, true);
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                SetPanelsActive(true, false);
                break;
        }
    }

    // Fungsi kompatibilitas jika dipanggil dari script lain
    public void GameOver()
    {
        RestartGame();
    }

    private void SetPanelsActive(bool pause, bool win)
    {
        if (pausePanel != null) pausePanel.SetActive(pause);
        if (gameWinPanel != null) gameWinPanel.SetActive(win);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}