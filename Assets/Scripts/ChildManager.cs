using System.Collections.Generic;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    [Tooltip("可拖拽的子物体列表，按顺序决定激活索引。若为空则使用 transform 子物体顺序。")]
    public List<GameObject> childrenOrder = new List<GameObject>();

    // 启用指定索引的子物体，其他子物体全部禁用
    public void ActivateOnlyChild(int childIndex)
    {
        if (childrenOrder != null && childrenOrder.Count > 0)
        {
            if (childIndex < 0 || childIndex >= childrenOrder.Count)
            {
                Debug.LogWarning($"ActivateOnlyChild: childIndex {childIndex} out of range [0, {childrenOrder.Count - 1}].");
                return;
            }

            for (int i = 0; i < childrenOrder.Count; i++)
            {
                GameObject child = childrenOrder[i];
                if (child != null)
                {
                    child.SetActive(i == childIndex);
                }
            }
            return;
        }

        int count = transform.childCount;
        if (count == 0)
            return;

        if (childIndex < 0 || childIndex >= count)
        {
            Debug.LogWarning($"ActivateOnlyChild: childIndex {childIndex} out of range [0, {count - 1}].");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                child.gameObject.SetActive(i == childIndex);
            }
        }
    }

    // 只开启名字匹配的子物体，其他都关闭
    public void ActivateOnlyChildByName(string childName)
    {
        if (childrenOrder != null && childrenOrder.Count > 0)
        {
            bool found = false;
            for (int i = 0; i < childrenOrder.Count; i++)
            {
                GameObject child = childrenOrder[i];
                if (child != null)
                {
                    bool active = child.name == childName;
                    child.SetActive(active);
                    if (active) found = true;
                }
            }
            if (!found)
            {
                Debug.LogWarning($"ActivateOnlyChildByName: no child named '{childName}' found in childrenOrder.");
            }
            return;
        }

        int count = transform.childCount;
        if (count == 0)
            return;

        bool foundDefault = false;
        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                bool active = child.name == childName;
                child.gameObject.SetActive(active);
                if (active) foundDefault = true;
            }
        }

        if (!foundDefault)
        {
            Debug.LogWarning($"ActivateOnlyChildByName: no child named '{childName}' found.");
        }
    }
}
