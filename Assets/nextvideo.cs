using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader1 : MonoBehaviour
{
    public void LoadTargetScene()
    {
        // ✅ 在跳转前查找并调用 SurveyManager 的保存方法
        SurveyManager surveyManager = FindObjectOfType<SurveyManager>();
        if (surveyManager != null)
        {
            surveyManager.SaveSurvey();
        }
        else
        {
            Debug.LogWarning("⚠️ 没有找到 SurveyManager，无法保存问卷！");
        }

        // ✅ 正常执行跳转逻辑
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("已经是最后一个场景了，无法跳转到下一个场景。");
        }
    }
}
