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
        UIManager.Instance?.UpdateRingCounter(RingsPassed, totalRingsRequired);
    }

    
    public void OnRingPassed()
    {
        if (LevelComplete || GameOver) return;

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
        UIManager.Instance?.ShowLevelComplete();
        Debug.Log("Level Complete - all rings passed!");
    }

    
    public void TriggerGameOver()
    {
        if (GameOver || LevelComplete) return;

        GameOver = true;
        Time.timeScale = 0f;
        UIManager.Instance?.ShowGameOver();
        Debug.Log("Game Over - out of fuel.");
    }

    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}