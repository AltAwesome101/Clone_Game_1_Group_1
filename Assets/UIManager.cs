using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("Ring Counter")]
    [SerializeField] private TextMeshProUGUI ringCounterText;
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [Header("Fuel Meter")]
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Image fuelFillImage;
    [SerializeField] private Color fuelHighColor = Color.green;
    [SerializeField] private Color fuelLowColor = Color.red;
    [SerializeField] private float lowFuelThreshold = 0.25f;
    [Header("Boost Meter")]
    [SerializeField] private Slider boostSlider;
    [SerializeField] private Image boostFillImage;
    [SerializeField] private Color boostReadyColor = Color.cyan;
    [SerializeField] private Color boostChargingColor = Color.gray;
    [Header("End Screens")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private TextMeshProUGUI finalTimeText;
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
    public void UpdateTimer(float elapsedSeconds)
    {
        if (timerText != null)
        {
            timerText.text = GameManager.FormatTime(elapsedSeconds);
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
    // value01: 0-1 boost meter fill. isReady: true once the boost can be triggered again.
    public void UpdateBoostMeter(float value01, bool isReady)
    {
        if (boostSlider != null)
        {
            boostSlider.minValue = 0f;
            boostSlider.maxValue = 1f;
            boostSlider.value = value01;
        }
        if (boostFillImage != null)
        {
            boostFillImage.color = isReady ? boostReadyColor : boostChargingColor;
        }
    }
    public void ShowLevelComplete(float finalTime)
    {
        if (levelCompletePanel != null) levelCompletePanel.SetActive(true);
        if (finalTimeText != null) finalTimeText.text = GameManager.FormatTime(finalTime);
    }
    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}