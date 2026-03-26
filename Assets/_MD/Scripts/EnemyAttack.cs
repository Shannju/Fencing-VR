using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public Transform attackTarget;
    public float attackSpeed = 3f;

    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f,4f));
            yield return StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        float time = 0;

        Vector3 start = transform.position;
        Vector3 target = attackTarget.position;

        while (time < 1)
        {
            time += Time.deltaTime * attackSpeed;

            transform.position = Vector3.Lerp(start, target, time);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transform.localPosition = startPos;
        transform.localRotation = startRot;
    }
}