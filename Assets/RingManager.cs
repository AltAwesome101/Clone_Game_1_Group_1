using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class RingManager : MonoBehaviour
{
    public static RingManager Instance { get; private set; }

    [Header("Ring Order")]
    [Tooltip("Drag all 30 ring GameObjects here, in flight order.")]
    [SerializeField] private List<Ring> rings = new List<Ring>();

    [Tooltip("How many rings stay visible at once. 2 = current ring + the next one.")]
    [SerializeField] private int visibleAhead = 2;

    [Header("Optional: on-screen guidance")]
    [Tooltip("Leave empty if you don't want a distance readout.")]
    [SerializeField] private TextMeshProUGUI nextRingDistanceText;

    private int currentIndex = 0;

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
        
        foreach (Ring ring in rings)
        {
            ring.gameObject.SetActive(false);
        }
        RevealWindow();
    }

    private void Update()
    {
        if (nextRingDistanceText == null) return;
        if (currentIndex >= rings.Count || GameManager.Instance.player == null) return;

        float distance = Vector3.Distance(
            GameManager.Instance.player.position,
            rings[currentIndex].transform.position);

        nextRingDistanceText.text = $"Next ring: {distance:F0} m";
    }

    private void RevealWindow()
    {
        for (int i = currentIndex; i < currentIndex + visibleAhead && i < rings.Count; i++)
        {
            rings[i].gameObject.SetActive(true);
        }
    }

    
    public void NotifyRingPassed(Ring ring)
    {
        
        if (currentIndex >= rings.Count || ring != rings[currentIndex]) return;

        rings[currentIndex].gameObject.SetActive(false);
        currentIndex++;

        GameManager.Instance.OnRingPassed();

        int newlyVisibleIndex = currentIndex + visibleAhead - 1;
        if (newlyVisibleIndex < rings.Count)
        {
            rings[newlyVisibleIndex].gameObject.SetActive(true);
        }
    }
}