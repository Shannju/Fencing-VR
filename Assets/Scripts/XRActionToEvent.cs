using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class XRActionToEvent : MonoBehaviour
{
    public InputActionReference pressButtonAction;
    public UnityEvent onPressed;

    private void OnEnable()
    {
        if (pressButtonAction == null || pressButtonAction.action == null)
        {
            Debug.LogError("pressButtonAction is null");
            return;
        }

        pressButtonAction.action.performed += OnActionPerformed;
        pressButtonAction.action.Enable();


        Debug.Log("PressButton Action ÒÑ°ó¶¨");
    }

    private void OnDisable()
    {
        if (pressButtonAction != null && pressButtonAction.action != null)
        {
            pressButtonAction.action.performed -= OnActionPerformed;
            pressButtonAction.action.Disable();
        }
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Action performed: " + context.action.name);
        onPressed?.Invoke();
    }
}