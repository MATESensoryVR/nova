using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            // ��������������ͷ
            transform.forward = Camera.main.transform.forward;
        }
    }
}
