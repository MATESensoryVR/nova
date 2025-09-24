using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SurveyManager : MonoBehaviour
{
    public GameObject questionPanel;

    [System.Serializable]
    public class SurveyAnswer
    {
        public string sceneName;
        public string toggleName;
        public string labelText;
        public string timestamp;
        public int entryID;  // 第几次进入项目
    }

    [System.Serializable]
    public class SurveyData
    {
        public List<SurveyAnswer> answers = new List<SurveyAnswer>();
    }

    private static HashSet<string> savedScenes = new HashSet<string>();
    private static int projectRunID = -1;

    private bool hasSaved = false;

    void Awake()
    {
        if (projectRunID == -1)
        {
            int lastRun = PlayerPrefs.GetInt("SurveyRunID", 0);
            projectRunID = lastRun + 1;
            PlayerPrefs.SetInt("SurveyRunID", projectRunID);
            PlayerPrefs.Save();

            Debug.Log($"🆕 本次运行编号 EntryID = {projectRunID}");
        }
    }

    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    void OnSceneUnload(Scene scene)
    {
        SaveSurvey();
    }

    public void SaveSurvey()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (hasSaved || savedScenes.Contains(currentScene))
        {
            Debug.Log($"📌 场景 {currentScene} 已保存过，跳过写入。");
            return;
        }

        hasSaved = true;
        savedScenes.Add(currentScene);

        Toggle[] toggles = questionPanel.GetComponentsInChildren<Toggle>();
        SurveyData data = new SurveyData();

        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                string label = "N/A";
                Text text = toggle.GetComponentInChildren<Text>();
                if (text != null)
                {
                    label = text.text;
                }
                else
                {
                    TMPro.TextMeshProUGUI tmp = toggle.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    if (tmp != null) label = tmp.text;
                }

                data.answers.Add(new SurveyAnswer
                {
                    sceneName = currentScene,
                    toggleName = toggle.name,
                    labelText = label,
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    entryID = projectRunID
                });

                Debug.Log($"✅ 记录：{toggle.name} - {label}");
                break;
            }
        }

        if (data.answers.Count == 0)
        {
            Debug.LogWarning("⚠️ No toggle selected.");
            return;
        }

        string filePath = Path.Combine(Application.persistentDataPath, "survey_log.csv");
        bool fileExists = File.Exists(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            if (!fileExists)
                writer.WriteLine("SceneName,ToggleName,LabelText,Timestamp,EntryID");

            foreach (var ans in data.answers)
            {
                writer.WriteLine($"{EscapeForCsv(ans.sceneName)},{EscapeForCsv(ans.toggleName)},{EscapeForCsv(ans.labelText)},{EscapeForCsv(ans.timestamp)},{ans.entryID}");
            }
        }

        Debug.Log($"📄 数据写入成功：{filePath}");
    }

    private string EscapeForCsv(string input)
    {
        if (string.IsNullOrEmpty(input)) return "\"\"";
        input = input.Replace("\"", "\"\"").Replace("\n", " ").Replace("\r", "");
        return $"\"{input}\"";
    }
}
