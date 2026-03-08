using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    [Header("生成设置")]
    public GameObject leafPrefab;      // 这里用来存放我们的蓝色树叶预制体
    public float spawnInterval = 0.8f; // 生成频率：每 0.8 秒掉落一片
    public float spawnAreaWidth = 1.5f;  // 生成范围：在 1.5 米的范围内随机掉落

    private float timer = 0f;

    void Update()
    {
        // 计时器不断累加时间
        timer += Time.deltaTime;

        // 当时间到达我们设置的频率时
        if (timer >= spawnInterval)
        {
            SpawnLeaf(); // 召唤一片树叶
            timer = 0f;  // 计时器清零，重新开始算时间
        }
    }

    void SpawnLeaf()
    {
        // 1. 在生成器的周围，随机找一个点
        Vector3 randomPos = new Vector3(
            Random.Range(-spawnAreaWidth, spawnAreaWidth),
            0, // Y轴偏移为0，因为高度由生成器本身决定
            Random.Range(-spawnAreaWidth, spawnAreaWidth)
        );

        // 2. 计算出最终的生成位置 (生成器本身的位置 + 随机偏移位置)
        Vector3 spawnPosition = transform.position + randomPos;

        // 3. 正式生成树叶！(并给它一个随机的初始旋转角度，这样每片树叶掉下来的姿态都不一样)
        Instantiate(leafPrefab, spawnPosition, Random.rotation);
    }
}