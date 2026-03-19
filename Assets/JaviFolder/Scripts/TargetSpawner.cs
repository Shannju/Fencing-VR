using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject VFX;
    public Transform[] spawnPoints;
    public TextMeshProUGUI timeText;
    public FencingSaluteDetector saluteDetector;
    public int maxTargets = 15;

    private GameObject currentTarget;
    private float currentTime;
    private int targetsNumber = 0;
    private int lastRandomIndex = 0;

    private float targetLifetime;
    private float spawnDelay;

    private bool isPlaying = false;
    private int score;
    private bool waitingForSalute = true;
    private GameMode currentMode;
    private DifficultyLevel currentDifficulty;


    public enum GameMode
    {
        Speedrun,
        Reaction
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard,
        Inferno
    }

    public UnityEvent GameEnded;

    //FOR USING:
    //
    //CALL PREPARE LEVEL FROM THE UI WHERE THE TYPE OF GAME IS SELECTED -- PrepareLevel(Gamemode.mode, DifficultyLevel.difficulty)
    //WHEN THE START STANCE IS MADE THE GAME WILL START
    //WHEN IT ENDS THE SCORE APEARS and Event is sent

    private void Start()
    {
        //SpawnTarget();
        targetsNumber = 15;
        currentTime = 0f;

        PrepareLevel(GameMode.Reaction, DifficultyLevel.Inferno);

    }

    void Update()
    {
        if (!isPlaying) return;

        if (targetsNumber >= maxTargets) return;

        if (currentMode == GameMode.Speedrun)
        {
            
            currentTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            int milliseconds = Mathf.FloorToInt((currentTime * 100) % 100);

            timeText.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
        }
        if (currentMode == GameMode.Reaction)
        {
            timeText.text = $"SCORE: {score}";
        }
    }
    public void StartGame(GameMode mode, DifficultyLevel difficulty)
    {
        currentMode = mode;
        SetDifficulty(difficulty);

        targetsNumber = 0;
        score = 0;
        currentTime = 0f;
        isPlaying = true;

        if (currentTarget != null)
            Destroy(currentTarget);

        SpawnTarget();
    }

    public void PrepareLevel(GameMode mode, DifficultyLevel difficulty)
    {
        StopGame();
        saluteDetector.ResetSalute();

        isPlaying = false;
        waitingForSalute = true;

        currentMode = mode;
        currentDifficulty = difficulty;

        if (currentTarget != null)
            Destroy(currentTarget);

    }

    public void OnSaluteCompleted()
    {
        if (!waitingForSalute) return;

        waitingForSalute = false;

        StartGame(currentMode, currentDifficulty);
    }


    private void SetDifficulty(DifficultyLevel level)
    {
        switch (level)
        {
            case DifficultyLevel.Easy:
                targetLifetime = 3f;
                spawnDelay = 1f;
                break;

            case DifficultyLevel.Medium:
                targetLifetime = 2f;
                spawnDelay = 0.8f;
                break;

            case DifficultyLevel.Hard:
                targetLifetime = 1.2f;
                spawnDelay = 0.6f;
                break;

            case DifficultyLevel.Inferno:
                targetLifetime = 0.7f;
                spawnDelay = 0.4f;
                break;
        }
    }

    private void SpawnTarget()
    {
        if (!isPlaying) return;

        if (targetsNumber >= maxTargets)
            return;

        int randomIndex = 0;
        while (lastRandomIndex == randomIndex)
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }

        Transform spawnPoint = spawnPoints[randomIndex];
        lastRandomIndex = randomIndex;

        currentTarget = Instantiate(targetPrefab, spawnPoint.position, spawnPoint.rotation);

        var button = currentTarget.GetComponentInChildren<XRSimpleInteractable>();
        if (button != null)
            button.selectEntered.AddListener((args) => TargetHit());

        //  ONLY for Reaction mode
        if (currentMode == GameMode.Reaction)
        {
            CancelInvoke(nameof(TargetMissed));
            Invoke(nameof(TargetMissed), targetLifetime);
        }

    }

    private void TargetMissed()
    {
        if (currentTarget == null) return;

        targetsNumber++; 

        if (targetsNumber >= maxTargets)
        {
            StopGame();
            return;
        }
        Destroy(currentTarget);
        currentTarget = null;

        Invoke(nameof(SpawnTarget), spawnDelay);
    }

    public void TargetHit()
    {

        if (!isPlaying) return;

        CancelInvoke(nameof(TargetMissed));

        targetsNumber++;
        if (targetsNumber >= maxTargets)
        {
            StopGame();
            return;
        }
        if (currentMode == GameMode.Reaction)
        {
            score++;
            // (update UI later)
        }
        if (currentTarget != null)
        {
            Vector3 particlePos = currentTarget.transform.position;
            particlePos.z += 0.2f;
            GameObject vfx = Instantiate(
                VFX,
                currentTarget.transform.position + -transform.forward * 0.2f,
                Quaternion.identity
            );

            Destroy(vfx, 3f); // destroy effect after playing

            Destroy(currentTarget);
            currentTarget = null;
        }
        
        Invoke(nameof(SpawnTarget), spawnDelay);
    }

    public void StopGame()
    {
        isPlaying = false;

        CancelInvoke();

        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }

        if (currentMode == GameMode.Speedrun)
        {
            timeText.text = $"FINAL TIME\n{timeText.text}";
        }
        else
        {
            timeText.text = $"FINAL SCORE: {score}";
        }

        GameEnded.Invoke();
    }

    public void StartTarget()
    {
        targetsNumber = 0;
        currentTime = 0f;
        Destroy(currentTarget);
        SpawnTarget();
    }
}
