using UnityEngine;
using System.Collections;

public class EnemyAttackPattern : MonoBehaviour
{
    public Transform[] attackTargets;
    public float attackSpeed = 3f;
    public float attackDelay = 2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
        StartCoroutine(AILoop());
    }

    IEnumerator AILoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(attackDelay);
            int attackIndex = Random.Range(0, attackTargets.Length);
            Transform target = attackTargets[attackIndex];
            yield return StartCoroutine(Attack(target));
        }
    }

    IEnumerator Attack(Transform target)
    {
        float t = 0;
        Vector3 start = transform.position;
        while(t < 1)
        {
            t += Time.deltaTime * attackSpeed;
            transform.position = Vector3.Lerp(start, target.position, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        transform.localPosition = startPos;
    }
}