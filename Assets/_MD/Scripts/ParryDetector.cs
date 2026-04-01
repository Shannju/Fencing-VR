using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParryDetector : MonoBehaviour
{
    //public float parryForce = 5f;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject[] hitParticles;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tip"))
        {
            Debug.Log("Tip!");
            scoreManager.AddScore(30);
            other.GetComponent<MeshRenderer>().material.color = Color.red;
            
            // Spawn particle at contact position
            Vector3 spawnPos = other.ClosestPoint(transform.position);
            int index = Random.Range(0, hitParticles.Length); 
            GameObject particle =  Instantiate(hitParticles[index], spawnPos, Quaternion.identity);
            particle.transform.localScale  = 0.5f * Vector3.one;
            /*Rigidbody enemyRb = other.GetComponent<Rigidbody>();

            if(enemyRb != null)
            {
                Vector3 deflectDir = (other.transform.position - transform.position).normalized;

                enemyRb.AddForce(deflectDir * parryForce, ForceMode.Impulse);
            }*/
        }
        if (other.CompareTag("Mid"))
        {
            Debug.Log("Mid!");
            scoreManager.AddScore(10);
            other.GetComponent<MeshRenderer>().material.color = Color.red;
            
            // Spawn particle at contact position
            Vector3 spawnPos = other.ClosestPoint(transform.position);
            int index = Random.Range(0, hitParticles.Length); 
            GameObject particle =  Instantiate(hitParticles[index], spawnPos, Quaternion.identity);
            particle.transform.localScale  = 0.5f * Vector3.one;
        }
        if (other.CompareTag("End"))
        {
            Debug.Log("End!");
            scoreManager.AddScore(50);
            other.GetComponent<MeshRenderer>().material.color = Color.red;
            
            // Spawn particle at contact position
            Vector3 spawnPos = other.ClosestPoint(transform.position);
            int index = Random.Range(0, hitParticles.Length); 
            GameObject particle =  Instantiate(hitParticles[index], spawnPos, Quaternion.identity);
            particle.transform.localScale  = 0.5f * Vector3.one;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Tip"))
        {
            Debug.Log("Tip!");
            scoreManager.AddScore(30);
            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            
            // Exact contact point
            Vector3 hitPoint = other.contacts[0].point;
            int index = Random.Range(0, hitParticles.Length); 
            Instantiate(hitParticles[index], hitPoint, Quaternion.identity);
            
        }
        if (other.gameObject.CompareTag("Mid"))
        {
            Debug.Log("Mid!");
            scoreManager.AddScore(10);
            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            
            Vector3 hitPoint = other.contacts[0].point;
            int index = Random.Range(0, hitParticles.Length); 
            Instantiate(hitParticles[index], hitPoint, Quaternion.identity);
        }
        if (other.gameObject.CompareTag("End"))
        {
            Debug.Log("End!");
            scoreManager.AddScore(50);
            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            
            Vector3 hitPoint = other.contacts[0].point;
            int index = Random.Range(0, hitParticles.Length); 
            Instantiate(hitParticles[index], hitPoint, Quaternion.identity);
        }
    }

   
}