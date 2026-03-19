using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class TutorialEventData
{
    [Tooltip("事件名称，可用于识别，UI 不强制显示")]
    public string eventName;

    [Tooltip("如果为空，则保持上次文本不变")]
    [TextArea(1, 5)]
    public string subtitle;

    [Tooltip("如果为空则不更新图标")]
    public Sprite boardIcon;

    [Header("UI 位置（空时保持当前位置）")]
    public bool overridePosition;
    public Vector3 uiPosition = new Vector3(0, 1.2f, 1.5f);
    public Vector3 uiRotation = Vector3.zero;

    [Tooltip("是否在此事件隐藏Next按钮")] 
    public bool hideNextButton;

    [Header("事件触发（可绑定对象/函数）")]
    public UnityEvent onEvent;
}

public class TutorialManager : MonoBehaviour
{
    [Header("核心引用")]
    public TextMeshProUGUI subtitleText;
    public Transform uiBoardTransform;
    public Image boardIconImage;

    [Header("流程控制引用")]
    [Tooltip("现在这个按钮会一直存在，充当 '跳过/下一页' 功能")]
    public GameObject nextButton;

    [Header("可配置教程事件: 你可以一组一组加, 自定义数量")]
    public List<TutorialEventData> tutorialEvents = new List<TutorialEventData>();

    [Header("默认位置 (当事件不覆盖位置时使用)")]
    public Vector3 defaultPosition = new Vector3(0, 1.2f, 1.5f);
    public Vector3 defaultRotation = Vector3.zero;

    [HideInInspector]
    public int currentEventIndex = 0;

    void Start()
    {
        if (tutorialEvents == null) tutorialEvents = new List<TutorialEventData>();
        if (tutorialEvents.Count == 0)
        {
            Debug.LogWarning("TutorialManager: tutorialEvents is empty. Use Add Empty Event or add events in Inspector.");
            return;
        }

        if (nextButton != null) nextButton.SetActive(true);
        UpdateUIForCurrentEvent();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceTutorial();
        }
    }

    public void AddEmptyEvent()
    {
        if (tutorialEvents == null) tutorialEvents = new List<TutorialEventData>();

        tutorialEvents.Add(new TutorialEventData
        {
            eventName = "",
            subtitle = "",
            boardIcon = null,
            overridePosition = false,
            uiPosition = defaultPosition,
            uiRotation = defaultRotation,
            hideNextButton = false
        });
    }

    public void AdvanceTutorial()
    {
        if (tutorialEvents == null || tutorialEvents.Count == 0) return;
        if (currentEventIndex >= tutorialEvents.Count - 1) return;
        currentEventIndex++;
        UpdateUIForCurrentEvent();
    }

    public void RestartTutorial()
    {
        if (tutorialEvents == null || tutorialEvents.Count == 0) return;
        currentEventIndex = 0;
        UpdateUIForCurrentEvent();
    }

    public void SetEventByName(string eventName)
    {
        if (tutorialEvents == null) return;
        for (int i = 0; i < tutorialEvents.Count; i++)
        {
            if (!string.IsNullOrEmpty(tutorialEvents[i].eventName) && tutorialEvents[i].eventName == eventName)
            {
                currentEventIndex = i;
                UpdateUIForCurrentEvent();
                return;
            }
        }
    }

    public string GetCurrentEventName()
    {
        if (tutorialEvents == null || tutorialEvents.Count == 0) return string.Empty;
        return tutorialEvents[currentEventIndex].eventName ?? string.Empty;
    }

    public bool IsCurrentEvent(string eventName)
    {
        if (string.IsNullOrEmpty(eventName) || tutorialEvents == null || tutorialEvents.Count == 0) return false;
        return string.Equals(GetCurrentEventName(), eventName, System.StringComparison.OrdinalIgnoreCase);
    }

    public void GoToEventIndex(int index)
    {
        if (tutorialEvents == null || index < 0 || index >= tutorialEvents.Count) return;
        currentEventIndex = index;
        UpdateUIForCurrentEvent();
    }

    public void UpdateCurrentSubtitle(string text)
    {
        if (subtitleText == null) return;
        if (!string.IsNullOrWhiteSpace(text))
        {
            subtitleText.text = text;
        }
    }

    void UpdateUIForCurrentEvent()
    {
        if (tutorialEvents == null || tutorialEvents.Count == 0)
        {
            Debug.LogWarning("TutorialManager: tutorialEvents is empty. 请在Inspector里添加事件。");
            return;
        }

        TutorialEventData data = tutorialEvents[currentEventIndex];

        // Next按钮默认可见，除非当前事件设置隐藏
        if (nextButton != null)
        {
            nextButton.SetActive(!data.hideNextButton);
        }

        if (subtitleText != null && !string.IsNullOrWhiteSpace(data.subtitle))
        {
            subtitleText.text = data.subtitle;
        }

        if (boardIconImage != null && data.boardIcon != null)
        {
            boardIconImage.sprite = data.boardIcon;
        }

        if (uiBoardTransform != null && data.overridePosition)
        {
            uiBoardTransform.position = data.uiPosition;
            uiBoardTransform.rotation = Quaternion.Euler(data.uiRotation);
        }

        if (data.onEvent != null)
        {
            data.onEvent.Invoke();
        }
    }
}
