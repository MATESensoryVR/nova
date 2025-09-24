using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadTargetScene()
    {
        SceneManager.LoadScene("Game");
    }
}
