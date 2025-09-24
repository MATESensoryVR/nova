using UnityEngine;

public class BasketChecker5 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ResettableObject1 obj = other.GetComponent<ResettableObject1>();

        if (obj != null)
        {
            string triggerTag = gameObject.tag; // ��ǰ���ӵ� tag ��Ϊ������Դ

            Debug.Log($"���ܲ��ݻ����壺{other.name} �� {triggerTag} ���ܣ�");

            obj.LogDestruction(triggerTag); // ��¼��־�������ݻ���Դ�� tag��
            Destroy(other.gameObject); // �ݻ�����
        }
    }
}