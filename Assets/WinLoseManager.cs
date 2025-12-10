using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{
    public void TitleScene()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;
        SceneManager.LoadScene(sceneName);
    }
}
