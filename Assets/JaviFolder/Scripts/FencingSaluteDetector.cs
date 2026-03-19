using UnityEngine;
using UnityEngine.Events;

public class FencingSaluteDetector : MonoBehaviour
{
    public Transform head;
    public Transform SaluteLocator;
    public Transform sword;
    public Transform hand;

    public float requiredHoldTime = 1f;

    public UnityEvent OnSaluteCompleted;

    private float poseTimer;
    private bool saluteCompleted = false;
    

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
                //poseTimer = -2f;
                OnSaluteCompleted.Invoke();
            }
        }
        else
        {
            poseTimer = 0f;
        }
    }

    public void ResetSalute()
    {
        poseTimer = 0f;
        saluteCompleted = false;
    }

    int CalculatePoseScore()
    {
        int score = 0;

        float distance = Vector3.Distance(SaluteLocator.position, head.position);

        Vector3 direction = (SaluteLocator.position - head.position).normalized;
        float dot = Vector3.Dot(head.forward, direction);

        if (distance < 0.6f && dot > 0.6f)
        {
            score++;
        }

        // Sword pointing upward
        if (Vector3.Angle(sword.forward, Vector3.up) < 30f)
            score++;

        return score;
    }

}