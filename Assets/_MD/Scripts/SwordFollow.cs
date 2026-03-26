using UnityEngine;

public class SwordFollow : MonoBehaviour
{
    public Transform controller;

    void Update()
    {
        transform.position = controller.position;
        transform.rotation = controller.rotation;
    }
}