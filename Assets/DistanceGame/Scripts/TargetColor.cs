using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TargetColor : MonoBehaviour
{
    public Renderer targetRenderer;
    private Color oriColor;
    //Renderer[] renderers;

    private void Start()
    {
        //renderers = targetRenderer.GetComponentsInChildren<Renderer>();
        oriColor = targetRenderer.material.color;
    }

    public void ChangeColor(Color color, float t)
    {
        Color c = Color.Lerp(oriColor, color, t);
        //foreach (var r in renderers)
        //{
        //    r.material.color = c;
        //}
        targetRenderer.material.color = c;
        //float intensity = Mathf.Lerp(0.5f, 2f, 1f - t);
        //targetRenderer.material.SetColor("_EmissionColor", c * intensity);
    }

    public Color GetOriColor()
    {
        return oriColor;
    }
}
