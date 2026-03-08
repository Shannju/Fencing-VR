using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform[] spawnPoints;
    public TextMeshProUGUI timeText;
    public int maxTargets = 15;

    private GameObject currentTarget;
    private float currentTime;
    private int targetsNumber = 0;
    private int lastRandomIndex = 0;

    private void Start()
    {
        //SpawnTarget();
        targetsNumber = 15;
        currentTime = 0f;
    }

    void Update()
    {
        if (targetsNumber >= maxTargets) return;

        currentTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime * 100) % 100);

        timeText.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    private void SpawnTarget()
    {
        if (targetsNumber >= maxTargets) return;
        //currentTime = 0f;
        int randomIndex = 0;
        while (lastRandomIndex == randomIndex)
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }
        
        Transform spawnPoint = spawnPoints[randomIndex];
        lastRandomIndex = randomIndex;

        currentTarget = Instantiate(targetPrefab, spawnPoint.position, spawnPoint.rotation);

        var button = currentTarget.GetComponentInChildren<XRSimpleInteractable>();
        if( button != null) 
            button.selectEntered.AddListener((args) => TargetHit());

    }

    public void TargetHit()
    {
        // Spawn next after small delay (optional but feels nicer)
        targetsNumber ++;
        Destroy(currentTarget);
        Invoke(nameof(SpawnTarget), 0.5f);
    }

    public void StartTarget()
    {
        targetsNumber = 0;
        currentTime = 0f;
        SpawnTarget();
    }
}
