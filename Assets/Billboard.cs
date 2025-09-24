using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            // 让物体面向摄像头
            transform.forward = Camera.main.transform.forward;
        }
    }
}
