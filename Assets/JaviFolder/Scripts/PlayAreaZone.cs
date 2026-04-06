using UnityEngine;
using UnityEngine.Events;

public class PlayAreaZone : MonoBehaviour
{
    public Transform player; // XR Origin or Head

    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    private bool isInside = true;

    void Update()
    {
        CheckPlayerInside();
    }

    void CheckPlayerInside()
    {
        Collider col = GetComponent<Collider>();

        bool inside = col.bounds.Contains(player.position);

        if (inside && !isInside)
        {
            isInside = true;
            OnPlayerEnter.Invoke();
        }
        else if (!inside && isInside)
        {
            isInside = false;
            OnPlayerExit.Invoke();
        }
    }

    public bool IsPlayerInside()
    {
        return isInside;
    }
}