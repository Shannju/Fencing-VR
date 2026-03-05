using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class XRInputProvider : MonoBehaviour
{
    public static XRInputProvider Instance;

    private InputDevice leftHand;
    private InputDevice rightHand;
    private InputDevice head;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!leftHand.isValid)
            leftHand = GetDevice(XRNode.LeftHand);

        if (!rightHand.isValid)
            rightHand = GetDevice(XRNode.RightHand);

        if (!head.isValid)
            head = GetDevice(XRNode.Head);
    }

    public InputDevice LeftHand => leftHand;
    public InputDevice RightHand => rightHand;
    public InputDevice Head => head;

    InputDevice GetDevice(XRNode node)
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(node, devices);
        return devices.Count > 0 ? devices[0] : default;
    }
}
