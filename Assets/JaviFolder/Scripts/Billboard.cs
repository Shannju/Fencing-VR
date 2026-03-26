using System.Security.Cryptography;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;


    void LateUpdate()
    {
        if (target == null) return;

        transform.LookAt(target);
        transform.Rotate(0, 180, 0);
    }
}
