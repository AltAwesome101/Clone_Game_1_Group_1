using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Ring : MonoBehaviour
{
    [Tooltip("Optional particle/VFX prefab spawned when this ring is passed. Leave empty to skip.")]
    [SerializeField] private GameObject passEffectPrefab;

    private void Reset()
    {
       
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (passEffectPrefab != null)
        {
            Instantiate(passEffectPrefab, transform.position, Quaternion.identity);
        }

        RingManager.Instance.NotifyRingPassed(this);
    }
}