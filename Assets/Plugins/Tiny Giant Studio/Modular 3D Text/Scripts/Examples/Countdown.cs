using System.Collections;
using UnityEngine;

namespace TinyGiantStudio.Text.Example
{
    public class Countdown : MonoBehaviour
    {
        [Header("Trigger")]
        [SerializeField] private Collider wordCollider;
        [SerializeField] private string triggerTag = "Player";

        [Header("Text")]
        [SerializeField] private Modular3DText modular3DText = null;
        [SerializeField] private string rewrittenText = "New Text";

        [Tooltip("开始重写前等待多久")]
        [SerializeField] private float rewriteDelay = 0.2f;

        [Tooltip("每个字出现的时间间隔")]
        [SerializeField] private float characterInterval = 0.1f;

        [Header("Options")]
        [SerializeField] private bool triggerOnlyOnce = true;

        private bool hasTriggered = false;
        private Coroutine rewriteRoutine;

        void Start()
        {
            if (wordCollider == null)
                wordCollider = GetComponent<Collider>();

            if (wordCollider != null)
                wordCollider.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (triggerOnlyOnce && hasTriggered)
                return;

            if (!other.CompareTag(triggerTag))
                return;

            if (modular3DText == null)
            {
                Debug.LogWarning("Modular3DText 没有赋值。");
                return;
            }

            hasTriggered = true;

            if (rewriteRoutine != null)
                StopCoroutine(rewriteRoutine);

            rewriteRoutine = StartCoroutine(RewriteTextRoutine());
        }

        IEnumerator RewriteTextRoutine()
        {
            if (characterInterval <= 0)
                characterInterval = 0.01f;

            // 先清空文字
            modular3DText.UpdateText("");

            // 等一下再开始“重写”
            yield return new WaitForSeconds(rewriteDelay);

            // 一个字一个字显示
            for (int i = 1; i <= rewrittenText.Length; i++)
            {
                modular3DText.UpdateText(rewrittenText.Substring(0, i));
                yield return new WaitForSeconds(characterInterval);
            }
        }

        public void ResetTrigger()
        {
            hasTriggered = false;
        }
    }
}