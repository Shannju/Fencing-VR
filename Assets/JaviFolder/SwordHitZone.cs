using UnityEngine;
using UnityEngine.Events;

public class SwordHitZone : MonoBehaviour
{
    public UnityEvent OnLeafHit;
    void OnTriggerEnter(Collider other)
    {
        //Vector3 closest = other.ClosestPoint(transform.position);
        if (other.CompareTag("Leaf")) { 
            OnLeafHit.Invoke();
            
            Destroy(other.gameObject);
        }
    }

}