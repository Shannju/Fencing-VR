using UnityEngine;
using UnityEngine.XR;


public class ButtonPressed : MonoBehaviour
{

    public XRInputProvider inputProvider;
    public SceneSwitcher sceneSwitcher;
    private InputDevice rightHand;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rightHand = inputProvider.RightHand;
    }

    // Update is called once per frame
    void Update()
    {
        if(!rightHand.isValid)
        {
            rightHand = inputProvider.RightHand;
            return;
        }

        if (rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        {
            sceneSwitcher.SwitchToScene("0Start");
        }
    }
}
