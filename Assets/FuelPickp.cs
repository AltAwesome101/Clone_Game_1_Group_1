using UnityEngine;


[RequireComponent(typeof(Collider))]
public class FuelPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float fuelAmount = 20f;
    [SerializeField] private GameObject collectEffectPrefab;
    [SerializeField] private bool respawns = false;
    [SerializeField] private float respawnDelay = 8f;

    [Header("Visual Juice (optional)")]
    [SerializeField] private float rotateSpeed = 90f;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        FuelManager.Instance.AddFuel(fuelAmount);

        if (collectEffectPrefab != null)
        {
            Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
        }

        if (respawns)
        {
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
    }
}