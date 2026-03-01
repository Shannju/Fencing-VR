using UnityEngine;

public class SwordHitZone : MonoBehaviour
{
    public VRSwordBend sword;

    void OnTriggerEnter(Collider other)
    {
        Vector3 closest = other.ClosestPoint(transform.position);
        //sword.StartContact(closest);
    }

    void OnTriggerExit(Collider other)
    {
        //sword.EndContact();
    }
}