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
    [Tooltip("Tarik Panel Game Over dari Hierarchy ke sini")]
    public GameObject gameOverUI; 
    public GameObject pausePanel;
    public GameObject gameWinPanel;

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
        // Fitur Pause toggle dengan tombol ESC atau P
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
                SetAllPanelsActive(false, false, false);
                break;

            case GameState.DieByBird:
                Debug.Log("GAME OVER: Terkena Burung!");
                HandleGameOver();
                break;

            case GameState.DieByCat:
                Debug.Log("GAME OVER: Terkena Kucing Kuning!");
                HandleGameOver();
                break;

            case GameState.DieByVoid:
                Debug.Log("GAME OVER: Jatuh ke Jurang / Void!");
                HandleGameOver();
                break;

            case GameState.GameWin:
                Debug.Log("YOU WIN!");
                Time.timeScale = 0f;
                SetAllPanelsActive(false, false, true);
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                SetAllPanelsActive(false, true, false);
                break;
        }
    }

    // Fungsi kompatibilitas agar panggil GameManager.instance.GameOver() lama tidak error
    public void GameOver()
    {
        ChangeState(GameState.DieByCat);
    }

    private void HandleGameOver()
    {
        Time.timeScale = 0f; // Membekukan waktu game
        SetAllPanelsActive(true, false, false);
    }

    private void SetAllPanelsActive(bool gameOver, bool pause, bool win)
    {
        if (gameOverUI != null) gameOverUI.SetActive(gameOver);
        if (pausePanel != null) pausePanel.SetActive(pause);
        if (gameWinPanel != null) gameWinPanel.SetActive(win);
    }

    // Fungsi Tombol UI
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
    }
}