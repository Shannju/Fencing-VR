using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTxt;
    private int score = 0;

    private void Start()
    {
        UpdateScoreUI();
    }

    // Add 'int amount' here so the method can receive the points
    public void AddScore(int amount)
    {
        score += amount; // This adds the specific value (30, 10, etc.) instead of just 1
        UpdateScoreUI();
    }

    // Helper method to keep things clean
    private void UpdateScoreUI()
    {
        if (scoreTxt != null)
        {
            scoreTxt.text = score.ToString();
        }
    }
}