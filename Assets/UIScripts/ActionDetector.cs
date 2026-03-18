using UnityEngine;

public class ActionDetector : MonoBehaviour
{
    [Tooltip("把场景里的 TutorialManager 拖到这里")]
    public TutorialManager tutorialManager;

    // 当有物体进入这个触发器区域时执行
    private void OnTriggerEnter(Collider other)
    {
        // 核心：只对上了暗号（标签为 PlayerSword）的物体有反应！
        if (other.CompareTag("PlayerSword"))
        {
            Debug.Log("检测到玩家的剑击中了判定区！");

            // 通知 UI 管理器进入下一关
            if (tutorialManager != null)
            {
                tutorialManager.AdvanceTutorial();
            }

            // 触发成功后，把自己隐藏掉（靶子消失）
            gameObject.SetActive(false);
        }
    }
}