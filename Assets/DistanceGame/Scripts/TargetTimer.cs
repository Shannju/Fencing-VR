using UnityEngine;
using UnityEngine.UI;

public class TargetTimer : MonoBehaviour
{
    public Image radialImage;

    private float lifetime;
    private float timer;

    public void Init(float life)
    {
        lifetime = life;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / lifetime;

        radialImage.fillAmount = 1f - t;
    }
}
