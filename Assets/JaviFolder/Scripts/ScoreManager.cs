using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTxt;

    private int score = 0;

    private void Start()
    {
        scoreTxt.text = score.ToString();
    }

    public void AddScore()
    {
        score++;
        scoreTxt.text = score.ToString();
    }
}
