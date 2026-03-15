using UnityEngine;

public class LeafController : MonoBehaviour
{
    [Header("击剑靶子 - 飘落设置")]
    public float fallSpeed = 0.4f;
    public float swaySpeed = 1.5f;
    public float swayWidth = 0.2f;

    [Header("交互与特效")]
    public float destroyYLevel = 0.1f;
    public GameObject hitParticlePrefab; // 放到这里的特效，击中后会爆开

    private float startX;
    private float startZ;
    private float randomOffset;
    private bool isHit = false; // 记录是否已经被击中，防止重复触发

    void Start()
    {
        startX = transform.position.x;
        startZ = transform.position.z;
        randomOffset = Random.Range(0f, 100f);

        // 如果没被击中，15秒后自动销毁
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        // 如果已经被击中（串在剑上了），就停止飘落的计算
        if (isHit) return;

        float newY = transform.position.y - fallSpeed * Time.deltaTime;
        float newX = startX + Mathf.Sin(Time.time * swaySpeed + randomOffset) * swayWidth;
        float newZ = startZ;

        transform.position = new Vector3(newX, newY, newZ);

        if (transform.position.y <= destroyYLevel)
        {
            Destroy(gameObject);
        }
    }

    // 碰撞检测魔法：当其他物体碰到树叶的 Trigger 时触发
    private void OnTriggerEnter(Collider other)
    {
        // 如果已经击中过，或者碰到的不是剑，就不管它
        if (isHit || !other.CompareTag("Sword")) return;

        // 标记为已击中！
        isHit = true;

        // 1. 爆出粒子特效
        if (hitParticlePrefab != null)
        {
            // 在树叶当前的位置生成粒子
            Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
        }

        // 2. 核心功能：串在剑上！(把树叶的父级改成剑)
        transform.SetParent(other.transform);

        // 3. 关闭树叶的物理碰撞，防止它一直在剑上疯狂抖动或引发物理Bug
        Collider leafCollider = GetComponent<Collider>();
        if (leafCollider != null) leafCollider.enabled = false;

        // 4. (可选) 3秒后销毁这片树叶。如果不加这句，你击中100片树叶，剑上就会挂着100片，会挡住玩家视线。
        //Destroy(gameObject, 3f);
    }
}