using UnityEngine;

public class BasketChecker2 : MonoBehaviour
{
    // ������������ tag������ "ball2"
    [SerializeField]
    private string allowedTag = "nova2";

    private void OnTriggerEnter(Collider other)
    {
        ResettableObject obj = other.GetComponent<ResettableObject>();

        if (obj != null)
        {
            if (other.CompareTag(allowedTag))
            {
                Debug.Log($"ƥ��ɹ���{allowedTag} �����ܣ�");
                Destroy(other.gameObject);
                // TODO: ��ӳɹ���Ч���ƷֵȲ���
            }
            else
            {
                Debug.Log($"ƥ��ʧ�ܣ�{other.tag} �������ܣ����彫���ã�");
                obj.ResetPosition(allowedTag);
            }
        }
    }
}
