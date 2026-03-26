using UnityEngine;

public class BladeControlManager : MonoBehaviour
{
    [Tooltip("���볡����� TutorialManager")]
    public TutorialManager tutorialManager;

    [Tooltip("��Ҫ�������ػӶ����β�����أ�")]
    public int requiredSwings = 4;

    private int currentSwings = 0;
    private bool lastHitLeft = false;
    private bool isFirstHit = true; // �����ж��ǲ��ǻӳ��ĵ�һ��

    // ��������ᱻ�����������Ӻ���
    public void HitBox(bool isLeft)
    {
        // ���ķ����ף�ֻ�е��̳��ƽ��� BladeControl �׶�ʱ�������Ӳ�������
        if (tutorialManager == null || !tutorialManager.IsCurrentEvent("BladeControl"))
            return;

        if (isFirstHit)
        {
            lastHitLeft = isLeft;
            isFirstHit = false;
            Debug.Log("�ӽ���ϰ���õģ���һ����");
        }
        else if (lastHitLeft != isLeft) // �����ο��ķ�����ϴβ�һ����������ʵ�����һӶ���
        {
            currentSwings++;
            lastHitLeft = isLeft;
            Debug.Log("�ӽ���ϰ����Ч�Ӷ���Ŀǰ������" + currentSwings);

            // ����ﵽ��Ŀ�����
            if (currentSwings >= requiredSwings)
            {
                Debug.Log("�ӽ���ϰ����ɣ�");
                tutorialManager.AdvanceTutorial(); // ���� UI ��������һ��
                gameObject.SetActive(false); // ������ɣ������Ұ���ȫ�����ص�
            }
        }
    }
}