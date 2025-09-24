using UnityEngine;

public class BasketChecker5 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ResettableObject1 obj = other.GetComponent<ResettableObject1>();

        if (obj != null)
        {
            string triggerTag = gameObject.tag; // 当前篮子的 tag 作为触发来源

            Debug.Log($"接受并摧毁物体：{other.name} 被 {triggerTag} 接受！");

            obj.LogDestruction(triggerTag); // 记录日志（包括摧毁来源的 tag）
            Destroy(other.gameObject); // 摧毁物体
        }
    }
}