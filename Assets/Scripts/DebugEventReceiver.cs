using UnityEngine;

public class DebugEventReceiver : MonoBehaviour
{
    public void PrintHello()
    {
        Debug.Log("DebugEventReceiver: event triggered!");
    }

    public void PrintMessage(string message)
    {
        Debug.Log("DebugEventReceiver: " + message);
    }
}