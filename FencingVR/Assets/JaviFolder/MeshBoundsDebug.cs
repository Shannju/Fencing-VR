using UnityEngine;

public class MeshBoundsDebug : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Debug.Log("Y Min: " + mesh.bounds.min.y);
        Debug.Log("Y Max: " + mesh.bounds.max.y);
    }
}