using UnityEngine;
using UnityEngine.SceneManagement; // 必须引入这个，才能控制场景切换

public class MainMenuController : MonoBehaviour
{
    [Header("场景名称设置")]
    [Tooltip("请在这里填入练习场景的准确名称")]
    public string practiceSceneName = "PracticeScene";

    [Tooltip("请在这里填入挑战场景的准确名称")]
    public string gameSceneName = "GameScene";

    // 这个方法绑定给“练习模式”按钮
    public void LoadPracticeScene()
    {
        Debug.Log("准备进入练习模式...");
        SceneManager.LoadScene(practiceSceneName);
    }

    // 这个方法绑定给“挑战模式”按钮
    public void LoadGameScene()
    {
        Debug.Log("准备进入挑战模式...");
        SceneManager.LoadScene(gameSceneName);
    }
}