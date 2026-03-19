using UnityEngine;
using UnityEngine.InputSystem;

public class VRInputHandler : MonoBehaviour
{
    public InputActionAsset inputActions;  // 用于存储 Input Actions 资源
    private InputAction pressButtonAction;  // 用于检测按钮按下的 Input Action

    void OnEnable()
    {
        // 获取 PressButton Action
        pressButtonAction = inputActions.FindActionMap("XRActions").FindAction("PressButton");

        // 添加调试信息，确保 Action 被正确启用
        if (pressButtonAction != null)
        {
            Debug.Log("PressButton Action 被成功找到！");
        }
        else
        {
            Debug.LogError("PressButton Action 未找到，请检查配置！");
        }

        // 绑定按钮按下事件
        pressButtonAction.performed += OnButtonPress;
        pressButtonAction.Enable();
    }

    void OnDisable()
    {
        // 移除事件监听器
        pressButtonAction.performed -= OnButtonPress;
        pressButtonAction.Disable();
    }

    // 处理按钮按下的事件
    private void OnButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 打印按下的控制器按钮名称
            Debug.Log("按钮按下: " + context.action.controls[0].name);

            // 获取按钮的值，检查哪个按钮被按下
            var buttonName = context.action.controls[0].name;

            // 根据按钮名称判断是哪一个按钮
            switch (buttonName)
            {
                case "primaryButton": // A 按钮
                    Debug.Log("A按钮被按下！");
                    break;

                case "secondaryButton": // B 按钮
                    Debug.Log("B按钮被按下！");
                    break;

                case "primary2DAxisButton": // X 按钮
                    Debug.Log("X按钮被按下！");
                    break;

                case "secondary2DAxisButton": // Y 按钮
                    Debug.Log("Y按钮被按下！");
                    break;

                default:
                    Debug.Log("未知按钮被按下：" + buttonName);
                    break;
            }
        }
    }
}