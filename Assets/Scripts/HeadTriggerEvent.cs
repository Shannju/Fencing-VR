
using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class HeadTriggerEvent : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private float requiredStayTime = 0.2f;
    [SerializeField] private bool mustBeHeld = true;
    [SerializeField] private bool triggerOnlyOnce = true;

    [Header("Tag Settings")]
    [SerializeField] private string triggerTag1 = "triggerTag1";
    [SerializeField] private string triggerTag2 = "triggerTag2";

    [Header("Position Lock Settings")]
    [SerializeField] private bool lockPositionOnTrigger = true;
    [SerializeField] private bool resetRotationOnTrigger = true;
    [SerializeField] private Vector3 targetRotation = Vector3.zero;

    [Header("Head Movement Settings")]
    [SerializeField] private bool enableHeadMovement = true;
    [SerializeField] private float headMovementDuration = 3f;
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Events")]
    public UnityEvent onTriggered;
    public UnityEvent onTriggeredTag1;
    public UnityEvent onTriggeredTag2;

    private bool hasTriggered = false;
    private float stayTimer = 0f;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable currentGrab;
    private Vector3 lockedPosition;
    private Quaternion lockedRotation;
    private Coroutine movementCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnlyOnce && hasTriggered) return;

        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = other.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab == null) return;

        if (mustBeHeld && !grab.isSelected) return;

        currentGrab = grab;
        stayTimer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerOnlyOnce && hasTriggered) return;
        if (currentGrab == null) return;

        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = other.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != currentGrab) return;

        if (mustBeHeld && !grab.isSelected)
        {
            stayTimer = 0f;
            return;
        }

        stayTimer += Time.deltaTime;

        if (stayTimer >= requiredStayTime)
        {
            hasTriggered = true;
            onTriggered?.Invoke();
            
            // 记录并锁定物体位置和旋转
            LockObjectPosition(grab);
            
            // 启动 Lerp 移动到头部位置
            if (enableHeadMovement)
            {
                if (movementCoroutine != null)
                {
                    StopCoroutine(movementCoroutine);
                }
                movementCoroutine = StartCoroutine(MoveToHeadPosition(grab));
            }
            
            // 根据tag触发不同的事件
            if (grab.CompareTag(triggerTag1))
            {
                onTriggeredTag1?.Invoke();
            }
            else if (grab.CompareTag(triggerTag2))
            {
                onTriggeredTag2?.Invoke();
            }
        }
        else if (lockPositionOnTrigger && hasTriggered)
        {
            // 如果已经触发，保持物体在锁定的位置和旋转
            if (grab != null && grab.transform != null)
            {
                grab.transform.position = lockedPosition;
                grab.transform.rotation = lockedRotation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = other.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null && grab == currentGrab)
        {
            currentGrab = null;
            stayTimer = 0f;
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
        currentGrab = null;
        stayTimer = 0f;
    }

    private void LockObjectPosition(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab)
    {
        if (grab == null || grab.transform == null) return;

        Transform grabTransform = grab.transform;
        
        // 记录当前位置
        lockedPosition = grabTransform.position;
        
        // 摆正物体旋转
        if (resetRotationOnTrigger)
        {
            lockedRotation = Quaternion.Euler(targetRotation);
            grabTransform.rotation = lockedRotation;
        }
        else
        {
            lockedRotation = grabTransform.rotation;
        }

        // 禁用刚体的物理运动
        Rigidbody rb = grab.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private IEnumerator MoveToHeadPosition(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab)
    {
        if (grab == null || grab.transform == null) yield break;

        Transform grabTransform = grab.transform;
        Vector3 startPosition = grabTransform.position;
        
        // 获取玩家头部位置（通过主摄像头）
        Vector3 headPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
        
        float elapsedTime = 0f;

        while (elapsedTime < headMovementDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / headMovementDuration);
            
            // 应用动画曲线
            float curveT = movementCurve.Evaluate(t);
            
            // Lerp 移动到头部位置
            grabTransform.position = Vector3.Lerp(startPosition, headPosition, curveT);
            
            yield return null;
        }

        // 确保最终位置准确
        grabTransform.position = headPosition;
    }}