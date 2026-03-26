using UnityEngine;

public class MoveToPointOnTrigger : MonoBehaviour
{
    [SerializeField] private Transform snapPoint;
    [SerializeField] private string targetTag = "Snappable";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        other.transform.SetParent(snapPoint, false);
        other.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}