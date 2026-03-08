using UnityEngine;
using UnityEngine.Events;

public class FencingSaluteDetector : MonoBehaviour
{
    public Transform head;
    public Transform swordTip;
    public Transform sword;
    public Transform hand;

    public float requiredHoldTime = 1f;

    public UnityEvent OnSaluteCompleted;

    private float poseTimer;
    private bool saluteCompleted;

    void Update()
    {
        if (saluteCompleted) return;

        int score = CalculatePoseScore();

        if (score >= 2)
        {
            poseTimer += Time.deltaTime;

            if (poseTimer >= requiredHoldTime)
            {
                saluteCompleted = true;
                OnSaluteCompleted.Invoke();
            }
        }
        else
        {
            poseTimer = 0f;
        }
    }

    int CalculatePoseScore()
    {
        int score = 0;

        float distance = Vector3.Distance(swordTip.position, head.position);

        Vector3 direction = (swordTip.position - head.position).normalized;
        float dot = Vector3.Dot(head.forward, direction);

        if (distance < 0.35f && dot > 0.6f)
        {
            score++;
        }

        // Sword pointing upward
        if (Vector3.Angle(sword.forward, Vector3.up) < 30f)
            score++;

        return score;
    }

}