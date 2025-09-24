using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using System.IO;

public class ResettableObject : MonoBehaviour
{
    [HideInInspector]
    public Vector3 startPosition;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    private int resetCount = 0;
    private string headsetID;
    private string saveKey;

    void Start()
    {
        // 获取设备唯一 ID
        headsetID = SystemInfo.deviceUniqueIdentifier;

        // 初始化位置和组件
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // 构建用于本地存储的 Key
        saveKey = GetSaveKey();

        // 加载重置次数
        resetCount = PlayerPrefs.GetInt(saveKey, 0);

        // 注册场景切换监听
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // 避免重复注册
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 当切换场景时自动清除计数
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        resetCount = 0;
        PlayerPrefs.SetInt(saveKey, 0);
        PlayerPrefs.Save();

        Debug.Log($"[{name}] 场景已切换到 {scene.name}，resetCount 已重置。");
    }

    public void ResetPosition(string triggerTag)
    {
        // 如果被抓住，强制释放
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            var interactor = grabInteractable.interactorsSelecting.Count > 0
                ? grabInteractable.interactorsSelecting[0]
                : null;

            if (interactor != null)
            {
                grabInteractable.interactionManager.SelectExit(interactor, grabInteractable);
            }
        }

        // 重置位置与旋转
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;

        // 清除物理状态
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 更新重置次数
        resetCount++;
        PlayerPrefs.SetInt(saveKey, resetCount);
        PlayerPrefs.Save();

        Debug.Log($"{name} 被重置，总次数: {resetCount}，来源: {triggerTag}");

        // 写入 CSV 日志
        WriteResetLogToCSV(triggerTag);
    }

    private string GetSaveKey()
    {
        return $"ResetCount_{headsetID}_{name}";
    }

    private void WriteResetLogToCSV(string triggerTag)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "reset_log.csv");
        bool fileExists = File.Exists(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            if (!fileExists)
            {
                writer.WriteLine("Time,HeadsetID,ObjectName,ResetCount,TriggeredByTag");
            }

            string logLine = $"{System.DateTime.Now:yyyy-MM-dd HH:mm:ss},{headsetID},{name},{resetCount},{triggerTag}";
            writer.WriteLine(logLine);
        }

        Debug.Log("CSV 日志已写入：" + filePath);
        resetCount = 0;
        PlayerPrefs.SetInt(saveKey, resetCount);
        PlayerPrefs.Save();
    }
}
