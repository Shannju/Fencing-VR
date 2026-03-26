using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    [Header("UI 引用")]
    // 这里改成了 GameObject！用来控制整个提示组合（背景+文字）
    public GameObject guideUI;
    public RawImage displayImage;       // 拖入你用来显示小人姿势的 RawImage (1)

    [Header("图片资源")]
    public Texture photoPoseTexture;    // 真人起势图片
    public Texture silhouetteTexture;   // 剪影图片

    void Start()
    {
        ShowInitialState();
    }

    // 状态1：初始状态
    public void ShowInitialState()
    {
        // 激活整个 Text_Guid 物体（连带它的子集文字也会一起显示）
        guideUI.SetActive(true);

        // 初始时隐藏主要展示区
        displayImage.gameObject.SetActive(false);
    }

    // 状态2：击中按钮后
    public void OnButtonHit()
    {
        // 1. 隐藏提示文字及背景
        guideUI.SetActive(false);

        // 2. 显示人物图片框，并换成真人图
        displayImage.gameObject.SetActive(true);
        displayImage.texture = photoPoseTexture;

        Debug.Log("按钮被击中，请玩家摆出起势姿势！");
    }

    // 状态3：动作识别成功后
    public void OnPoseRecognized()
    {
        // 1. 直接切换为剪影图
        displayImage.texture = silhouetteTexture;

        Debug.Log("姿势识别成功！游戏开始！");
    }
}