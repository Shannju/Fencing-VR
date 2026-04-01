using UnityEngine;

public class myBillboard : MonoBehaviour
{
    private Transform camTransform;

    private void Start()
    {
        // ��ȡ�������Transform
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        // ʹCanvas������ʼ��������
        Vector3 direction = camTransform.position - transform.position;
        direction.y = 0; // ������Y����ת

        // ������ת
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation *= Quaternion.Euler(0, 180, 0); // ��Y���ϼ���180�ȵ���ת

        // Ӧ����ת
        transform.rotation = rotation;
    }
}
