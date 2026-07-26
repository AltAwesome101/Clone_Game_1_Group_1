using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Ring Counter")]
    [SerializeField] private TextMeshProUGUI ringCounterText;

    [Header("Fuel Meter")]
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Image fuelFillImage;
    [SerializeField] private Color fuelHighColor = Color.green;
    [SerializeField] private Color fuelLowColor = Color.red;
    [SerializeField] private float lowFuelThreshold = 0.25f;

    [Header("End Screens")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateRingCounter(int passed, int total)
    {
        if (ringCounterText != null)
        {
            ringCounterText.text = $"Rings: {passed} / {total}";
        }
    }

    public void UpdateFuelMeter(float current, float max)
    {
        if (fuelSlider != null)
        {
            fuelSlider.maxValue = max;
            fuelSlider.value = current;
        }

        if (fuelFillImage != null)
        {
            float t = max > 0f ? current / max : 0f;
            fuelFillImage.color = t <= lowFuelThreshold ? fuelLowColor : fuelHighColor;
        }
    }

    public void ShowLevelComplete()
    {
        if (levelCompletePanel != null) levelCompletePanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}