using UnityEngine;

public class ParryDetector : MonoBehaviour
{
    //public float parryForce = 5f;
    [SerializeField] private ScoreManager scoreManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemySword"))
        {
            Debug.Log("Parry!");
            scoreManager.AddScore();
            /*Rigidbody enemyRb = other.GetComponent<Rigidbody>();

            if(enemyRb != null)
            {
                Vector3 deflectDir = (other.transform.position - transform.position).normalized;

                enemyRb.AddForce(deflectDir * parryForce, ForceMode.Impulse);
            }*/
        }
    }
}