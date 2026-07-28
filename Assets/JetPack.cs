using UnityEngine;

// Attach to the jetpack object. Requires a Collider set to "Is Trigger".
public class JetPack : MonoBehaviour
{
    [Tooltip("Empty child transform on the player marking where the jetpack should snap to (e.g. spine/back bone).")]
    public Transform backAttachPoint;

    public Vector3 localPositionOffset = Vector3.zero;
    public Vector3 localRotationOffset = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        FlyController flyController = other.GetComponent<FlyController>();
        if (flyController != null)
        {
            flyController.EquipJetpack();
        }

        AttachToPlayer(other.transform);
    }

    private void AttachToPlayer(Transform player)
    {
        Transform attachPoint = backAttachPoint;

        // fallback: look for a child named "JetpackAttachPoint" on the player rig
        if (attachPoint == null)
        {
            attachPoint = player.Find("JetpackAttachPoint");
        }

        // last resort: parent directly to the player root
        if (attachPoint == null)
        {
            attachPoint = player;
        }

        // disable the trigger so it doesn't fire again once equipped
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        transform.SetParent(attachPoint);
        transform.localPosition = localPositionOffset;
        transform.localEulerAngles = localRotationOffset;
    }
}
