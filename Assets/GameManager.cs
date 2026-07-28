using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Reference")]
    [Tooltip("Drag the player character (from the Hierarchy) here.")]
    public Transform player;

    [Header("Level Settings")]
    [SerializeField] private int totalRingsRequired = 30;

    public int RingsPassed { get; private set; }
    public int TotalRingsRequired => totalRingsRequired;
    public bool LevelComplete { get; private set; }
    public bool GameOver { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        RingsPassed = 0;
        LevelComplete = false;
        GameOver = false;

        SetGameplayCursor();

        UIManager.Instance?.UpdateRingCounter(RingsPassed, totalRingsRequired);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void SetGameplayCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetMenuCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnRingPassed()
    {
        if (LevelComplete || GameOver)
            return;

        RingsPassed++;
        UIManager.Instance?.UpdateRingCounter(RingsPassed, totalRingsRequired);

        if (RingsPassed >= totalRingsRequired)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        LevelComplete = true;
        Time.timeScale = 0f;

        SetMenuCursor();

        UIManager.Instance?.ShowLevelComplete();
        Debug.Log("Level Complete - all rings passed!");
    }

    public void TriggerGameOver()
    {
        if (GameOver || LevelComplete)
            return;

        GameOver = true;
        Time.timeScale = 0f;

        SetMenuCursor();

        UIManager.Instance?.ShowGameOver();
        Debug.Log("Game Over - out of fuel.");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SetGameplayCursor();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}