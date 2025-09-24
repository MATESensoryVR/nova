using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using System.IO;

public class ResettableObject1 : MonoBehaviour
{
    [HideInInspector]
    public Vector3 startPosition;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    private string headsetID;

    void Start()
    {
        headsetID = SystemInfo.deviceUniqueIdentifier;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ����ʹ�� resetCount����˲�����
    }

    /// <summary>
    /// д��ݻټ�¼��־
    /// </summary>
    public void LogDestruction(string triggerTag)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "destroy_log.csv");
        bool fileExists = File.Exists(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            if (!fileExists)
            {
                writer.WriteLine("Time,HeadsetID,ObjectName,DestroyedByTag");
            }

            string logLine = $"{System.DateTime.Now:yyyy-MM-dd HH:mm:ss},{headsetID},{name},{triggerTag}";
            writer.WriteLine(logLine);
        }

        Debug.Log($"�Ѽ�¼�ݻ���־��{name} �� {triggerTag} �ݻ�");
    }
}
