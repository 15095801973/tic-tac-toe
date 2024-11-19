using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // 要切换到的场景名称
    public string sceneName;

    // 切换场景的按钮点击事件
    public void SwitchScene()
    {
        // 加载新场景
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
        // 在Unity编辑器中停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 在独立应用程序中退出游戏
        Application.Quit();
        #endif
    }
}