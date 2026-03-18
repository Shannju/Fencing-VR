using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialState
    {
        Intro,
        BladeControl,
        Attack,
        Defence,
        Parry,
        ParryRiposte,
        FinalPractice,
        Completed
    }

    [Header("核心引用")]
    public TextMeshProUGUI subtitleText;
    public Transform uiBoardTransform;
    public Image boardIconImage;

    [Header("流程控制引用")]
    [Tooltip("现在这个按钮会一直存在，充当'跳过/下一页'功能")]
    public GameObject nextButton;
    public GameObject part1Targets;
    public GameObject part2Target;

    [Header("各阶段的 Figma 图标 (拖入Sprite)")]
    public Sprite iconIntro;
    public Sprite iconBladeControl;
    public Sprite iconAttack;
    public Sprite iconDefence;
    public Sprite iconParry;         // 新增：Part 4 图标
    public Sprite iconParryRiposte;  // 新增：Part 5 图标
    public Sprite iconFinal;         // 新增：Final 图标
    public Sprite iconCompleted;     // 新增：完成图标

    [Header("UI 位置设置")]
    public Vector3 centerPosition = new Vector3(0, 1.2f, 1.5f);
    public Vector3 centerRotation = new Vector3(0, 0, 0);
    public Vector3 sidePosition = new Vector3(0.8f, 0.9f, 1.3f);
    public Vector3 sideRotation = new Vector3(0, -30f, 0);

    public TutorialState currentState = TutorialState.Intro;

    void Start()
    {
        if (part1Targets != null) part1Targets.SetActive(false);
        if (part2Target != null) part2Target.SetActive(false);

        // 游戏一开始，确保 Next 按钮是显示的
        if (nextButton != null) nextButton.SetActive(true);

        UpdateUIForCurrentState();
    }

    void Update()
    {
        // 你的空格键跳页神器！
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceTutorial();
        }
    }

    public void AdvanceTutorial()
    {
        if (currentState == TutorialState.Completed) return;
        currentState++;
        UpdateUIForCurrentState();
    }

    void UpdateUIForCurrentState()
    {
        // 🌟 【关键修改】：每次更新状态时，都强行保证 Next 按钮是显示的！
        if (nextButton != null) nextButton.SetActive(true);

        switch (currentState)
        {
            case TutorialState.Intro:
                subtitleText.text = "Welcome, fencer.\nIn this tutorial, you'll learn how to control your blade.\nIn fencing, the hand does most of the work.\nSmall, precise movements decide whether you attack, defend, or score a touch.\nLet's begin.";
                PlaceUI(centerPosition, centerRotation);
                if (boardIconImage != null) boardIconImage.sprite = iconIntro;
                break;

            case TutorialState.BladeControl:
                subtitleText.text = "Part 1 - Blade Control\nFirst, hold your blade in front of you.\nYour arm should be relaxed, with the tip of your sword pointing toward your opponent.\nTry moving your blade slightly from side to side.";
                PlaceUI(sidePosition, sideRotation);
                if (boardIconImage != null) boardIconImage.sprite = iconBladeControl;

                if (part1Targets != null) part1Targets.SetActive(true);
                if (part2Target != null) part2Target.SetActive(false);
                break;

            case TutorialState.Attack:
                subtitleText.text = "Part 2 - Attack\nThe most basic attack is a thrust.\nExtend your arm forward and aim the tip of your blade toward your opponent's target.\nTry attacking now.";
                PlaceUI(new Vector3(0.8f, 0.9f, 1.8f), sideRotation);
                if (boardIconImage != null) boardIconImage.sprite = iconAttack;

                if (part1Targets != null) part1Targets.SetActive(false);
                if (part2Target != null) part2Target.SetActive(true);
                break;

            case TutorialState.Defence:
                subtitleText.text = "Part 3 - Defence\nWhen your opponent attacks, you must defend yourself.\nInstead of moving your whole body, you can deflect the blade with your sword.\nStay focused. An attack is coming.";
                PlaceUI(sidePosition, sideRotation);
                if (boardIconImage != null) boardIconImage.sprite = iconDefence;

                if (part2Target != null) part2Target.SetActive(false);
                break;

            // 🌟 【修复】：补全了后面所有关卡的文字，现在按空格键绝对有反应了！
            case TutorialState.Parry:
                subtitleText.text = "Part 4 - Parry\nDeflect the incoming blade to protect your target area.\nMove your blade sideways to block.";
                PlaceUI(sidePosition, sideRotation);
                if (boardIconImage != null) boardIconImage.sprite = iconParry;
                break;

            case TutorialState.ParryRiposte:
                subtitleText.text = "Part 5 - Parry and Riposte\nA good defense creates an opening.\nParry the attack, then immediately thrust back!";
                PlaceUI(sidePosition, sideRotation);
                if (boardIconImage != null) boardIconImage.sprite = iconParryRiposte;
                break;

            case TutorialState.FinalPractice:
                subtitleText.text = "Final Practice\nTime to combine everything.\nDefend yourself, find the opening, and strike!";
                PlaceUI(sidePosition, sideRotation);
                if (boardIconImage != null) boardIconImage.sprite = iconFinal;
                break;

            case TutorialState.Completed:
                subtitleText.text = "Tutorial Completed!\nYou have mastered the basics. Return to the main menu when ready.";
                PlaceUI(centerPosition, centerRotation);
                if (boardIconImage != null) boardIconImage.sprite = iconCompleted;

                // 只有在全部结束时，我们才把 Next 按钮隐藏（或者你可以把它变成“返回大厅”按钮）
                if (nextButton != null) nextButton.SetActive(false);
                break;
        }
    }

    void PlaceUI(Vector3 targetPos, Vector3 targetRot)
    {
        if (uiBoardTransform != null)
        {
            uiBoardTransform.position = targetPos;
            uiBoardTransform.rotation = Quaternion.Euler(targetRot);
        }
    }
}