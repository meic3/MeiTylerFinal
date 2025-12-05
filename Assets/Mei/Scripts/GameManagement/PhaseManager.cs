using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using Unity.VisualScripting;

public class PhaseManager : MonoBehaviour
{
    public WaveManager[] waves; 

    public bool isInGame;
    public BugGenerator bugGenerator;

    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI phaseText; 

    [SerializeField]
    private TextMeshProUGUI winLoseText; 
    private float remainingTime;
    private float setTime = 30;
    private int currentWaveNum = 0;

    [SerializeField]
    private GameObject timerUI;

    public bool isPaused = false;
    public bool inTutorial = false;

    void Awake()
    {
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            isPaused = true;
            inTutorial = true;
        }

        else
        {
        }
    } 

    void Start()
    {
        winLoseText.text = "";
        remainingTime = setTime;
        bugGenerator.canGenerate = false;
        isInGame = false;
        UpdatePhaseText(); 
    }

    void Update()
    {
        if (!isInGame)
        {
            PrepTime();
        }
        else
        {
            InGame();
        }
    }

    public void InGame()
    {
        timerUI.SetActive(false);
        isInGame = true;
        
        if (bugGenerator.IsWaveComplete())
        {
    
            currentWaveNum++;
            
            if (currentWaveNum >= waves.Length)
            {
                // beateeeeed theeeeee levellllllll
                winLoseText.text = "You Win!!!!";
                            Debug.Log("Win");
            }
            
            isInGame = false;
            remainingTime = setTime;
            UpdatePhaseText();
        }
    }

    public void SkipPrepTime()
    {
        if (!isInGame) 
        {
            remainingTime = 0; 
        }
    }

    public void PrepTime()
    {
        if (remainingTime <= 0)
        {
            remainingTime = setTime;
            isInGame = true;
            
            if (currentWaveNum < waves.Length)
            {
                bugGenerator.StartNewWave(waves[currentWaveNum]);
                UpdatePhaseText();
            }
        }
        else
        {
            timerUI.SetActive(true);
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void UpdatePhaseText()
    {
        if (phaseText != null)
        {
            if (isInGame)
            {
                phaseText.text = "Wave " + (currentWaveNum + 1);
            }
            else
            {
                phaseText.text = "Prep Time";
            }
        }
    }

    public void LoseGame()
    {
        winLoseText.text = "You Lost!!!!!!!";
    }
}