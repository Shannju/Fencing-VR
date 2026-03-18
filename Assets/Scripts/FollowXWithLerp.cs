using UnityEngine;

public class FollowXWithLerp : MonoBehaviour
{
    public Transform target;   // 要跟随的目标
    public float smoothSpeed = 5f; // 平滑程度（越大越快）

    void Update()
    {
        if (target == null) return;

        Vector3 currentPos = transform.position;
        float targetX = target.position.x;

        // 只在X轴插值
        float newX = Mathf.Lerp(currentPos.x, targetX, smoothSpeed * Time.deltaTime);

        transform.position = new Vector3(newX, currentPos.y, currentPos.z);
    }
}