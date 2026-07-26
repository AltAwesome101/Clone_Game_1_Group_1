using UnityEngine;


public class FuelManager : MonoBehaviour
{
    public static FuelManager Instance { get; private set; }

    [Header("Fuel Settings")]
    [SerializeField] private float maxFuel = 100f;
    [SerializeField] private float startingFuel = 100f;
    [SerializeField] private float drainRatePerSecond = 2f;

    public float CurrentFuel { get; private set; }
    public float MaxFuel => maxFuel;
    public bool HasFuel => CurrentFuel > 0f;

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
        CurrentFuel = Mathf.Clamp(startingFuel, 0f, maxFuel);
        UIManager.Instance?.UpdateFuelMeter(CurrentFuel, maxFuel);
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver || GameManager.Instance.LevelComplete) return;
        if (CurrentFuel <= 0f) return;

        CurrentFuel = Mathf.Max(0f, CurrentFuel - drainRatePerSecond * Time.deltaTime);
        UIManager.Instance?.UpdateFuelMeter(CurrentFuel, maxFuel);

        if (CurrentFuel <= 0f)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }

    
    public void AddFuel(float amount)
    {
        if (GameManager.Instance.GameOver || GameManager.Instance.LevelComplete) return;

        CurrentFuel = Mathf.Min(maxFuel, CurrentFuel + amount);
        UIManager.Instance?.UpdateFuelMeter(CurrentFuel, maxFuel);
    }
}