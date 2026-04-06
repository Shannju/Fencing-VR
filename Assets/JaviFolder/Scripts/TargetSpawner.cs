using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject VFX;
    [Header("流程状态控制 (拖拽刚才建的三个State物体进来)")]
    public GameObject stateGuideText;    // 对应 State_1_GuideText
    public GameObject stateSaluteGuide;  // 对应 State_2_SaluteGuide
    public GameObject stateGameVisuals;  // 对应 State_3_GameVisuals
    //public Transform cameraTransform;
    public Transform[] spawnPoints;
    public TextMeshProUGUI timeText;
    public FencingSaluteDetector saluteDetector;
    public PlayAreaZone gameZone;
    public int maxTargets = 15;


    private GameObject currentTarget;
    private float currentTime;
    private int targetsNumber = 0;
    private int lastRandomIndex = 0;

    //Change difficulty
    private float targetLifetime;
    private float spawnDelay;
    private Vector3 targetScale;

    private bool isPlaying = false;
    private int score;
    private bool waitingForSalute = true;
    private GameMode currentMode;
    private DifficultyLevel currentDifficulty;

    private float difficultyProgress = 0f;

    [Header("Survival Settings")]
    public float minLifetime = 0.7f;
    public float maxLifetime = 2.5f;

    public float minSpawnDelay = 0.2f;
    public float maxSpawnDelay = 1.2f;

    public float minScale = 0.8f;
    public float maxScale = 1.5f;



    public enum GameMode
    {
        Speedrun,
        Reaction,
        Survival
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
        // === 【新增】：初始化时，只打开第一阶段的文字，关掉其他画面 ===
        if (stateGuideText != null) stateGuideText.SetActive(true);
        if (stateSaluteGuide != null) stateSaluteGuide.SetActive(false);
        if (stateGameVisuals != null) stateGameVisuals.SetActive(false);

        // === 【保留】：保留你原来的重置参数逻辑 ===
        targetsNumber = 15;
        currentTime = 0f;

        // === 【删除/注释】：把下面这句原来直接开始游戏的代码注释掉！===
        // PrepareLevel(GameMode.Reaction, DifficultyLevel.Inferno); 
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
        if (currentMode == GameMode.Reaction || currentMode == GameMode.Survival)
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

        if (mode == GameMode.Survival)
        {
            score = 0;
            difficultyProgress = 0f;

            targetLifetime = maxLifetime;
            spawnDelay = maxSpawnDelay;
            targetScale = new Vector3(maxScale, 1f, maxScale);
        }

        SpawnTarget();
    }

    public void PrepareLevel(GameMode mode, DifficultyLevel difficulty)
    {
        // 
        StopGame();
        saluteDetector.ResetSalute();

        isPlaying = false;
        waitingForSalute = true;

        currentMode = mode;
        currentDifficulty = difficulty;

        // 
        if (stateGuideText != null) stateGuideText.SetActive(false);
        if (stateSaluteGuide != null) stateSaluteGuide.SetActive(true);
        if (stateGameVisuals != null) stateGameVisuals.SetActive(false);

        // 
        if (currentTarget != null)
            Destroy(currentTarget);
    }

    public void OnSaluteCompleted()
    {
        if (!waitingForSalute) return;
        if (!gameZone.IsPlayerInside())
        {
            saluteDetector.ResetSalute();
            return;
        }

        waitingForSalute = false;

        if (stateSaluteGuide != null) stateSaluteGuide.SetActive(false);
        if (stateGameVisuals != null) stateGameVisuals.SetActive(true);

        StartGame(currentMode, currentDifficulty);
    }


    private void SetDifficulty(DifficultyLevel level)
    {
        switch (level)
        {
            case DifficultyLevel.Easy:
                targetLifetime = 3f;
                spawnDelay = 1f;
                targetScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;

            case DifficultyLevel.Medium:
                targetLifetime = 2f;
                spawnDelay = 0.8f;
                targetScale = new Vector3(1.3f, 1.3f, 1.3f);
                break;

            case DifficultyLevel.Hard:
                targetLifetime = 1.2f;
                spawnDelay = 0.6f;
                targetScale = new Vector3(1.1f, 1.1f, 1.1f);
                break;

            case DifficultyLevel.Inferno:
                targetLifetime = 0.7f;
                spawnDelay = 0.4f;
                targetScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
        }
    }

    private void SpawnTarget()
    {
        if (!isPlaying) return;

        
        if (targetsNumber >= maxTargets)
            return;

        if (currentMode == GameMode.Reaction || currentMode == GameMode.Survival)
        {
            CancelInvoke(nameof(TargetMissed));
            Invoke(nameof(TargetMissed), targetLifetime);
        }
        if (currentTarget != null) return;

        int randomIndex = 0;
        while (lastRandomIndex == randomIndex)
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }

        Transform spawnPoint = spawnPoints[randomIndex];
        lastRandomIndex = randomIndex;

        currentTarget = Instantiate(targetPrefab, spawnPoint.position, spawnPoint.rotation);
        currentTarget.transform.localScale = targetScale;
        currentTarget.GetComponent<TargetTimer>().Init(targetLifetime);
        //currentTarget.GetComponent<Billboard>().target = cameraTransform;

        var button = currentTarget.GetComponentInChildren<XRSimpleInteractable>();
        if (button != null)
            button.selectEntered.AddListener((args) => TargetHit());

        /*if (currentMode == GameMode.Reaction || currentMode == GameMode.Survival)
        {
            CancelInvoke(nameof(TargetMissed));
            Invoke(nameof(TargetMissed), targetLifetime);
        }*/
        

    }

    private void TargetMissed()
    {
        if (currentTarget == null) return;
        if (currentMode == GameMode.Survival)
        {
            StopGame();

            timeText.text = $"GAME OVER\nScore: {score}";
            return;
        }
        else
        {
            targetsNumber++;

            if (targetsNumber >= maxTargets)
            {
                StopGame();
                return;
            }
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
        if (currentMode == GameMode.Survival)
        {
            score++;

            UpdateSurvivalDifficulty(); 
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
        if(currentMode == GameMode.Speedrun)
            Invoke(nameof(SpawnTarget), 0.5f);
        else
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

    public void PauseTarget()
    {
        isPlaying = false;
        CancelInvoke();
    }

    public void ResumeTarget()
    {
        isPlaying = true;

        SpawnTarget();
    }

    void UpdateSurvivalDifficulty()
    {
        // progress grows with score
        difficultyProgress = score * 0.05f;

        float t = Mathf.Clamp01(1f - Mathf.Exp(-score * 0.08f));

        // harder over time
        targetLifetime = Mathf.Lerp(maxLifetime, minLifetime, t * 0.7f);
        spawnDelay = Mathf.Lerp(maxSpawnDelay, minSpawnDelay, t);

        float scaleValue = Mathf.Lerp(maxScale, minScale, t * 0.5f);
        targetScale = new Vector3(scaleValue, 1f, scaleValue);
    }
}
