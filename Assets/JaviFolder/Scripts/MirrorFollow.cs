using UnityEngine;

public class MirrorFollow : MonoBehaviour
{
    public Transform playerCamera;
    public Transform mirror;

    public float mirrorWidth = 6f;
    public float cameraDistance = 1.5f;

    

    void LateUpdate()
    {
        // Player position in mirror local space
        Vector3 localPlayer = mirror.InverseTransformPoint(playerCamera.position);

        // Clamp movement within mirror width
        float clampedX = Mathf.Clamp(localPlayer.x, -mirrorWidth * 0.5f, mirrorWidth * 0.5f);

        // Camera position relative to mirror
        Vector3 localCamPos = new Vector3(clampedX, 0f, cameraDistance);

        // Convert back to world space
        transform.position = mirror.TransformPoint(localCamPos) + Vector3.up * 0.6f;
        

        // Look toward the mirror center (not the player)
        //transform.LookAt(playerCamera.position);
    }
}
