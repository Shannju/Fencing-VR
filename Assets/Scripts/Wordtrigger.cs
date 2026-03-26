using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TinyGiantStudio.Text.Example
{
    public class WordTrigger : MonoBehaviour
    {
        [SerializeField] private Collider wordCollider;
        [SerializeField] private string triggerTag = "Player"; // 可在Inspector中修改触发tag
        [SerializeField] private float fallDuration = 1f; // 下落持续时间
        [SerializeField] private float fallDistance = 5f; // 下落距离
        [Space]
        [SerializeField] private UnityEvent onWordHit = new UnityEvent(); // 公开事件，可在Inspector中配置

        private Vector3 originalPosition;
        private bool isFalling = false;

        void Start()
        {
            // 保存原始位置
            originalPosition = transform.position;

            // 如果没有指定collider，则尝试获取
            if (wordCollider == null)
                wordCollider = GetComponent<Collider>();
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"触发碰撞！碰撞物体: {other.gameObject.name}");

            // 恢复这里的判断逻辑：只有当碰到物体的 Tag 等于 triggerTag，且当前没有在下落时，才执行下落
            if (other.CompareTag(triggerTag) && !isFalling)
            {
                Debug.Log("开始下落！");
                StartCoroutine(FallAndRespawn());
            }
        }

        IEnumerator FallAndRespawn()
        {
            isFalling = true;

            // 下落动画
            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;
            Vector3 endPosition = startPosition - new Vector3(0, fallDistance, 0);

            while (elapsedTime < fallDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fallDuration;
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            // 确保到达终点
            transform.position = endPosition;

            // 等待一段时间（可选）
            yield return new WaitForSeconds(0.5f);

            // 触发事件
            onWordHit.Invoke();

            // 重新生成回到原位
            transform.position = originalPosition;
            isFalling = false;
        }

        // 用于外部重置
        public void ResetPosition()
        {
            StopAllCoroutines();
            transform.position = originalPosition;
            isFalling = false;
        }
    }
}