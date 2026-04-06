using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class DistanceTrainerScript : MonoBehaviour
{
    public Transform playerHead;
    public Transform target;
    public TargetColor targetColor;
    public FencingSaluteDetector saluteDetector;
    public PlayAreaZone gameZone;
    public AudioSource source;
    public GameObject StateSalute;

    [Header("Distance settings")]
    public float idealDist = 2.0f;
    public float tolerance = 0.3f;

    [Header("Movement")]
    public float moveSpeed = 1.0f;
    public float moveRange = 1.0f;

    [Header("Scoring")]
    public float score;
    public float scorePerSecond = 10f;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI TimeUI;


    [Header("Timer")]
    public float gameDuration = 30f;

    private float timeRemaining;

    private Vector3 startPos;
    private bool playGame = false;
    private float nextMoveTime;
    private float jumpTarget;
    private bool isJumping;
    private float pingTarget;
    private float pingTimer;
    private float smoothTarget;
    private float smoothTimer;

    public MovementType movementType = MovementType.Static;

    public enum MovementType
    {
        Static,
        Sine,
        PingPong,
        RandomJump,
        SmoothRandom
    }

    void Start()
    {
        startPos = target.position;
    }

    private void Update()
    {
        if (!playGame) return;

        MoveTarget();
        CheckDistance();
        ShowScore();
        TimeToFinish();
    }


    public void StartDistanceTraining()
    {
        //might add  setActive(true) + spawn position ... 
        if (!gameZone.IsPlayerInside())
        {
            saluteDetector.ResetSalute();
            return;
        }

        StateSalute.SetActive(false);

        timeRemaining = gameDuration;
        score = 0;
        playGame = true;
        //start sounds time
        source.Play();
    }

    public void StopDistanceTraining()
    {
        //might add setActive(false) 
        playGame = false;
        //finish time sound
        source.Stop();
    }

    public void PauseTarget()
    {
        playGame = false;
        source.Pause();
    }

    public void ResumeTarget()
    {
        playGame = true;

        source.UnPause();
    }

    private void TimeToFinish()
    {
        timeRemaining -= Time.deltaTime;
        ShowTime(Mathf.Max(0,timeRemaining));
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            StopDistanceTraining();
        }
    }

    private void MoveTarget()
    {
        //Sin wave for move -- future use probabilities?
        float offset;
        switch (movementType)
        {
            case MovementType.Static:
                target.position = startPos;
                break;
            case MovementType.Sine:
                offset = Mathf.Sin(Time.time * moveSpeed) * moveRange;
                target.position = startPos + target.forward * offset;
                break;
            case MovementType.PingPong:
                MovePingPong();
                break;
            case MovementType.RandomJump:
                MoveRandomJump();
                break;
            case MovementType.SmoothRandom:
                MoveSmoothRandom();
                break;
        }
        
        
    }
    void MovePingPong()
    {
        pingTimer -= Time.deltaTime;

        if (pingTimer <= 0f)
        {
            pingTarget = (pingTarget > 0f) ? -moveRange : moveRange;
            pingTimer = 0.5f; // pause time
        }

        float currentZ = Vector3.Dot(target.position - startPos, target.forward);
        float newZ = Mathf.Lerp(currentZ, pingTarget, Time.deltaTime * moveSpeed * 3f);

        target.position = startPos + target.forward * newZ;
    }

    void MoveRandomJump()
    {
        if (!isJumping)
        {
            jumpTarget = Random.Range(-moveRange, moveRange);
            isJumping = true;
        }

        float currentZ = Vector3.Dot(target.position - startPos, target.forward);
        float newZ = Mathf.Lerp(currentZ, jumpTarget, Time.deltaTime * moveSpeed * 8f);

        target.position = startPos + target.forward * newZ;

        if (Mathf.Abs(newZ - jumpTarget) < 0.05f)
        {
            isJumping = false;
        }
    }

    void MoveSmoothRandom()
    {
        smoothTimer -= Time.deltaTime;

        if (smoothTimer <= 0f)
        {
            smoothTarget = Random.Range(-moveRange, moveRange);
            smoothTimer = Random.Range(1f, 2f); // how long before changing
        }

        float currentZ = Vector3.Dot(target.position - startPos, target.forward);
        float newZ = Mathf.Lerp(currentZ, smoothTarget, Time.deltaTime * moveSpeed);

        target.position = startPos + target.forward * newZ;
    }

    private void CheckDistance()
    {
        //Vector3 toTarget = target.position - playerHead.position;
        //float distance = Vector3.Dot(toTarget, target.forward);
        float distance = Vector3.Distance(target.position, playerHead.position);

        float diff = Mathf.Abs(distance - idealDist);
        float t = Mathf.Clamp01(diff / tolerance);

        
        if(distance < idealDist - tolerance)
        {
            //Too close , text/different color
            targetColor.ChangeColor(Color.red, t);
        }
        else if(distance > idealDist + tolerance)
        {
            //Too far , text/color
            targetColor.ChangeColor(Color.yellow, t);
        }
        else 
        {
            score += scorePerSecond * Time.deltaTime;
            targetColor.ChangeColor(Color.green, t);
        }
        // Have a UI slider that represents the distance?
    }

    public float GetScore()
    {
        return score;
    }

    private void ShowScore()
    {
        textScore.text = "score:\n" + Mathf.Floor(score).ToString();
    }

    private void ShowTime(float t)
    {
        TimeUI.text = "time:\n" + t.ToString("F2");
    }

    public void SetTraining(float moveSpeed, float moveRange, float scorePerSecond)
    {
        this.moveSpeed = moveSpeed;
        this.moveRange = moveRange;
        this.scorePerSecond = scorePerSecond;
    }

    public void SetMovementType(MovementType type)
    {
        saluteDetector.ResetSalute();
        movementType = type;
        //Do this with the pose
        //StartDistanceTraining();
    }
}
