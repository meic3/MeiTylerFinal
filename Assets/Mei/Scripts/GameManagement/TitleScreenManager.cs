using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject levelSelection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelSelection.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void OpenLevelSelection()
    {
        levelSelection.active = true;
    }

    public void CloseSelector()
    {
        levelSelection.active = false;
    }

    public void GotoLevel1(int levelNum)
    {
        SFXManager.Instance.PlaySound(SFXManager.SoundType.Select);
        SceneManager.LoadScene("Level1");
    }

    public void GotoLevel2(int levelNum)
    {
        SceneManager.LoadScene("Level2");
    }

    public void GotoLevel3(int levelNum)
    {
        SceneManager.LoadScene("Level3");
    }

    public void GotoTutorial(int levelNum)
    {
        SceneManager.LoadScene("Tutorial");
    }
}
