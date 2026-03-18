using UnityEngine;

public class BladeControlManager : MonoBehaviour
{
    [Tooltip("拖入场景里的 TutorialManager")]
    public TutorialManager tutorialManager;

    [Tooltip("需要左右来回挥动几次才算过关？")]
    public int requiredSwings = 4;

    private int currentSwings = 0;
    private bool lastHitLeft = false;
    private bool isFirstHit = true; // 用来判断是不是挥出的第一剑

    // 这个方法会被左右两个靶子呼叫
    public void HitBox(bool isLeft)
    {
        // 核心防作弊：只有当教程推进到 BladeControl 阶段时，砍靶子才算数！
        if (tutorialManager.currentState != TutorialManager.TutorialState.BladeControl)
            return;

        if (isFirstHit)
        {
            lastHitLeft = isLeft;
            isFirstHit = false;
            Debug.Log("挥剑练习：好的，第一剑！");
        }
        else if (lastHitLeft != isLeft) // 如果这次砍的方向和上次不一样（代表真实的左右挥动）
        {
            currentSwings++;
            lastHitLeft = isLeft;
            Debug.Log("挥剑练习：有效挥动！目前次数：" + currentSwings);

            // 如果达到了目标次数
            if (currentSwings >= requiredSwings)
            {
                Debug.Log("挥剑练习：完成！");
                tutorialManager.AdvanceTutorial(); // 告诉 UI 面板进入下一关
                gameObject.SetActive(false); // 任务完成，把左右靶子全都隐藏掉
            }
        }
    }
}