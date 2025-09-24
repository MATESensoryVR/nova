using UnityEngine;

public class BasketChecker4 : MonoBehaviour
{
    // 设置允许进入的 tag，比如 "ball1"
    [SerializeField]
    private string allowedTag = "nova4";

    private void OnTriggerEnter(Collider other)
    {
        ResettableObject obj = other.GetComponent<ResettableObject>();

        if (obj != null)
        {
            if (other.CompareTag(allowedTag))
            {
                Debug.Log($"匹配成功：{allowedTag} 被接受！");
                Destroy(other.gameObject);
                // TODO: 添加成功音效、计分等操作
            }
            else
            {
                Debug.Log($"匹配失败：{other.tag} 不被接受，物体将重置！");
                obj.ResetPosition(allowedTag);
            }
        }
    }
}
