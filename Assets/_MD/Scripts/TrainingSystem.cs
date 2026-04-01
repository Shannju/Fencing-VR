using UnityEngine;

public class TrainingSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] highlightedTargets; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    
    public void SetTarget()
    {
        foreach (var obj in highlightedTargets)
        {
            obj.SetActive(false);
        }
        
        int randomIndex = Random.Range(0, highlightedTargets.Length);
        highlightedTargets[randomIndex].SetActive(true);
    }

    public void Resetcolor()
    {
        foreach (var obj in highlightedTargets)
        {
            obj.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}
