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

    [Header("Timer")]
    public float ElapsedTime { get; private set; }

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
        ElapsedTime = 0f;
        SetGameplayCursor();
        UIManager.Instance?.UpdateRingCounter(RingsPassed, totalRingsRequired);
        UIManager.Instance?.UpdateTimer(ElapsedTime);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

       
        if (!LevelComplete && !GameOver)
        {
            ElapsedTime += Time.deltaTime;
            UIManager.Instance?.UpdateTimer(ElapsedTime);
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
        UIManager.Instance?.ShowLevelComplete(ElapsedTime);
        Debug.Log($"Level Complete - all rings passed! Time: {FormatTime(ElapsedTime)}");
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

   
    public static string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}