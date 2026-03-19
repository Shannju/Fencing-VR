using UnityEngine;

public class MoveX : MonoBehaviour
{
    public float step = 1f;

    // 向 +X 方向移动 step
    public void MovePositiveX()
    {
        transform.position += Vector3.right * step;
    }

    // 向 -X 方向移动 step
    public void MoveNegativeX()
    {
        transform.position += Vector3.left * step;
    }

    // 向 +Z 方向移动 step
    public void MovePositiveZ()
    {
        transform.position += Vector3.forward * step;
    }

    // 向 -Z 方向移动 step
    public void MoveNegativeZ()
    {
        transform.position += Vector3.back * step;
    }

    // 可以传入任意值：正数向 +X，负数向 -X
    public void MoveByX(float delta)
    {
        transform.position += Vector3.right * delta;
    }

    // 可以传入任意值：正数向 +Z，负数向 -Z
    public void MoveByZ(float delta)
    {
        transform.position += Vector3.forward * delta;
    }
}
