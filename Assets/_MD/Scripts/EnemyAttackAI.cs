using UnityEngine;
using System.Collections;

public class EnemyAttackAI : MonoBehaviour
{
    [Header("Attack Targets")]
    public Transform leftTarget;
    public Transform rightTarget;
    public Transform topTarget;
    public Transform thrustTarget;

    [Header("Attack Settings")]
    public float attackSpeed = 3f;
    public float attackDelay = 2f;
    public float rotationAngle = 90f;
    public float returnSpeed = 2f;
    
    [SerializeField] private TrainingSystem trainingSystem;

    public bool gamePaused = false;

    Vector3 startPos;
    Quaternion startRot;

    enum AttackType
    {
        LeftSlash,
        RightSlash,
        TopAttack,
        Thrust
    }

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;

        //StartCoroutine(AILoop());
        StartTraining();
    }

    public void StartTraining()
    {
        gamePaused = false;
        StartCoroutine(AILoop());
    }

    public void PauseTraining()
    {
        gamePaused = true;
    }

    public void ResumeTraining()
    {
        gamePaused = false;
    }

    public void StopTraining()
    {
        gamePaused = true;
        StopCoroutine(AILoop());

    }

    IEnumerator AILoop()
    {
        while (!gamePaused)
        {
            yield return new WaitForSeconds(attackDelay);
          

            AttackType attack = (AttackType)Random.Range(0, 4);

            switch (attack)
            {
                case AttackType.LeftSlash:
                    yield return StartCoroutine(SlashAttack(leftTarget, Vector3.up));
                    break;

                case AttackType.RightSlash:
                    yield return StartCoroutine(SlashAttack(rightTarget, -Vector3.up));
                    break;

                case AttackType.TopAttack:
                    yield return StartCoroutine(SlashAttack(topTarget, Vector3.right));
                    break;

                case AttackType.Thrust:
                    yield return StartCoroutine(ThrustAttack(thrustTarget));
                    break;
            }

            yield return StartCoroutine(ReturnToStart());
        }
    }

    IEnumerator SlashAttack(Transform target, Vector3 rotationAxis)
    {
        if (gamePaused) yield return null;
        float t = 0;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);

        while (t < 1)
        {
            t += Time.deltaTime * attackSpeed;

            transform.position = Vector3.Lerp(startPosition, target.position, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator ThrustAttack(Transform target)
    {
        if (gamePaused) yield return null;

        float t = 0;

        Vector3 startPosition = transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * attackSpeed * 1.5f;

            transform.position = Vector3.Lerp(startPosition, target.position, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator ReturnToStart()
    {
        if (gamePaused) yield return null;
        yield return new WaitForSeconds(1f);
        float t = 0;

        Vector3 currentPos = transform.localPosition;
        Quaternion currentRot = transform.localRotation;

        while (t < 1)
        {
            t += Time.deltaTime * returnSpeed;

            transform.localPosition = Vector3.Lerp(currentPos, startPos, t);
            transform.localRotation = Quaternion.Slerp(currentRot, startRot, t);

            yield return null;
        }
        trainingSystem.SetTarget();
        trainingSystem.Resetcolor();
    }
}