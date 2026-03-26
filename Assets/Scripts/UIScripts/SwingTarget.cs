using UnityEngine;

public class SwingTarget : MonoBehaviour
{
    [Tooltip("如果这是左边的靶子，请打勾；右边就不打勾")]
    public bool isLeftBox;

    [Tooltip("拖入父物体身上的 BladeControlManager")]
    public BladeControlManager manager;

    private void OnTriggerEnter(Collider other)
    {
        // 再次确认暗号！只认贴了 "PlayerSword" 标签的剑
        if (other.CompareTag("PlayerSword"))
        {
            manager.HitBox(isLeftBox);
        }
    }
}