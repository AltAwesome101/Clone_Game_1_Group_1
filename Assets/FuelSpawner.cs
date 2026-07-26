using System.Collections.Generic;
using UnityEngine;


public class FuelSpawner : MonoBehaviour
{
    [Header("Spawn Volume")]
    [Tooltip("Resize/position this collider to cover the airspace pickups can spawn in. It only defines a volume - it doesn't need to be solid, and this script forces it to a trigger automatically.")]
    [SerializeField] private BoxCollider spawnBounds;

    [Header("What to Spawn")]
    [SerializeField] private GameObject fuelPickupPrefab;
    [Tooltip("How many pickups to place. Turn this up or down to scale the level.")]
    [SerializeField] private int pickupCount = 15;

    [Header("Placement Rules")]
    [Tooltip("Layers that count as solid geometry a pickup shouldn't spawn inside (your buildings/ground layer).")]
    [SerializeField] private LayerMask obstacleLayers;
    [Tooltip("Roughly the pickup's own radius - used to check it won't clip into geometry.")]
    [SerializeField] private float clearanceRadius = 1f;
    [Tooltip("Minimum distance kept between spawned pickups so they don't cluster together.")]
    [SerializeField] private float minSpacing = 5f;
    [Tooltip("Optional: drag ring transforms (or anything else) here to keep pickups clear of them too.")]
    [SerializeField] private List<Transform> pointsToAvoid = new List<Transform>();
    [Tooltip("How many times to retry a point before giving up on that pickup.")]
    [SerializeField] private int maxAttemptsPerPickup = 30;

    private readonly List<GameObject> spawned = new List<GameObject>();

    private void Reset()
    {
        
        spawnBounds = GetComponent<BoxCollider>();
    }

    private void Awake()
    {
        
        if (spawnBounds != null) spawnBounds.isTrigger = true;
    }

    private void Start()
    {
        SpawnAll();
    }

    
    public void SpawnAll()
    {
        ClearSpawned();

        if (spawnBounds == null || fuelPickupPrefab == null)
        {
            Debug.LogWarning("FuelSpawner is missing its spawn bounds or pickup prefab.");
            return;
        }

        List<Vector3> placedPositions = new List<Vector3>();

        for (int i = 0; i < pickupCount; i++)
        {
            if (TryFindSpawnPoint(placedPositions, out Vector3 point))
            {
                GameObject pickup = Instantiate(fuelPickupPrefab, point, Quaternion.identity, transform);
                spawned.Add(pickup);
                placedPositions.Add(point);
            }
            else
            {
                Debug.LogWarning($"FuelSpawner: no clear spot found for pickup {i + 1}/{pickupCount} after {maxAttemptsPerPickup} attempts.");
            }
        }
    }

    private bool TryFindSpawnPoint(List<Vector3> placedPositions, out Vector3 result)
    {
        Bounds bounds = spawnBounds.bounds;

        for (int attempt = 0; attempt < maxAttemptsPerPickup; attempt++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y), 
                Random.Range(bounds.min.z, bounds.max.z));

            if (Physics.CheckSphere(candidate, clearanceRadius, obstacleLayers))
                continue; 

            if (IsTooCloseToExisting(candidate, placedPositions))
                continue;

            result = candidate;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private bool IsTooCloseToExisting(Vector3 candidate, List<Vector3> placedPositions)
    {
        foreach (Vector3 existing in placedPositions)
        {
            if (Vector3.Distance(existing, candidate) < minSpacing) return true;
        }

        foreach (Transform avoid in pointsToAvoid)
        {
            if (avoid != null && Vector3.Distance(avoid.position, candidate) < minSpacing) return true;
        }

        return false;
    }

    private void ClearSpawned()
    {
        foreach (GameObject obj in spawned)
        {
            if (obj != null) Destroy(obj);
        }
        spawned.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        
        if (spawnBounds == null) return;

        Bounds b = spawnBounds.bounds;
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.DrawCube(b.center, b.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(b.center, b.size);
    }
}