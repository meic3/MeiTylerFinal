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

    public void GotoLevel(int levelNum)
    {
        SceneManager.LoadScene("Mei_PrototypeScene");
    }
}
