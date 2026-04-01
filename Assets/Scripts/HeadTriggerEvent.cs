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
    [SerializeField] private string triggerTag3 = "triggerTag3";
    [SerializeField] private string triggerTag4 = "triggerTag4"; // 新增 Tag 4

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
    public UnityEvent onTriggeredTag3;
    public UnityEvent onTriggeredTag4; // 新增 Event 4

    private bool hasTriggered = false;
    private float stayTimer = 0f;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable currentGrab;
    private Vector3 lockedPosition;
    private Quaternion lockedRotation;
    private Coroutine movementCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnlyOnce && hasTriggered) return;

        var grab = other.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab == null) return;

        if (mustBeHeld && !grab.isSelected) return;

        currentGrab = grab;
        stayTimer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        // 核心逻辑：如果已经触发且需要锁定，则强制更新位置
        if (hasTriggered)
        {
            if (lockPositionOnTrigger && currentGrab != null)
            {
                currentGrab.transform.position = lockedPosition;
                currentGrab.transform.rotation = lockedRotation;
            }
            return;
        }

        if (currentGrab == null) return;

        var grab = other.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != currentGrab) return;

        // 如果中途松开且必须按住，则重置计时
        if (mustBeHeld && !grab.isSelected)
        {
            stayTimer = 0f;
            return;
        }

        stayTimer += Time.deltaTime;

        if (stayTimer >= requiredStayTime)
        {
            ExecuteTriggerSequence(grab);
        }
    }

    private void ExecuteTriggerSequence(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab)
    {
        hasTriggered = true;
        onTriggered?.Invoke();

        // 1. 锁定物理状态
        LockObjectPosition(grab);

        // 2. 启动平滑移动
        if (enableHeadMovement)
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            movementCoroutine = StartCoroutine(MoveToHeadPosition(grab));
        }

        // 3. 根据 Tag 触发对应事件
        CheckTagAndInvoke(grab);
    }

    private void CheckTagAndInvoke(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab)
    {
        if (grab.CompareTag(triggerTag1)) onTriggeredTag1?.Invoke();
        else if (grab.CompareTag(triggerTag2)) onTriggeredTag2?.Invoke();
        else if (grab.CompareTag(triggerTag3)) onTriggeredTag3?.Invoke();
        else if (grab.CompareTag(triggerTag4)) onTriggeredTag4?.Invoke(); // 判定 Tag 4
    }

    private void OnTriggerExit(Collider other)
    {
        var grab = other.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null && grab == currentGrab)
        {
            // 只有在没触发成功的情况下才清除引用，触发成功后需要保留引用来维持“位置锁定”
            if (!hasTriggered)
            {
                currentGrab = null;
                stayTimer = 0f;
            }
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
        currentGrab = null;
        stayTimer = 0f;
        if (movementCoroutine != null) StopCoroutine(movementCoroutine);
    }

    private void LockObjectPosition(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab)
    {
        if (grab == null) return;

        lockedPosition = grab.transform.position;
        lockedRotation = resetRotationOnTrigger ? Quaternion.Euler(targetRotation) : grab.transform.rotation;

        grab.transform.rotation = lockedRotation;

        Rigidbody rb = grab.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // 触发后转为运动学模式，防止物理抖动
        }
    }

    private IEnumerator MoveToHeadPosition(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab)
    {
        if (grab == null) yield break;

        Vector3 startPosition = grab.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < headMovementDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / headMovementDuration);
            float curveT = movementCurve.Evaluate(t);

            // 实时获取头部位置（防止玩家移动）
            Vector3 headPos = Camera.main != null ? Camera.main.transform.position : transform.position;

            grab.transform.position = Vector3.Lerp(startPosition, headPos, curveT);

            // 重要：同步锁定位置，防止 OnTriggerStay 的锁定逻辑造成画面闪烁
            lockedPosition = grab.transform.position;

            yield return null;
        }

        // 最终对齐
        if (Camera.main != null)
        {
            grab.transform.position = Camera.main.transform.position;
            lockedPosition = grab.transform.position;
        }
    }
}