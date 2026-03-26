using UnityEngine;

public class SwordParry : MonoBehaviour
{
    public EnemyAttackAI enemyAI;
    public float parryForce = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemySword"))
        {
            Debug.Log("PARRY SUCCESS!");

            // Stop enemy attack
            enemyAI.StopAllCoroutines();

            // Push sword away
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 pushDir = (other.transform.position - transform.position).normalized;
                rb.AddForce(pushDir * parryForce, ForceMode.Impulse);
            }
            
           
            enemyAI.StartCoroutine("ReturnToStart");
            // Restart enemy AI
            enemyAI.StartCoroutine("AILoop");
        }
    }
}