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

    [SerializeField]
    private GameObject WinLoseImage ; 
    private float remainingTime;
    private float setTime = 30;
    private int currentWaveNum = 0;

    [SerializeField]
    //private GameObject timerUI;

    public bool isPaused = false;
    public bool inTutorial = false;

    void Awake()
    {
        PlayerMoney.money = 0;
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            isPaused = true;
            bugGenerator.isPaused = true;
            inTutorial = true;
        }

        else
        {
        }
    } 

    void Start()
    {
        WinLoseImage.SetActive(false);
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
        //timerUI.SetActive(false);
        isInGame = true;
        
        if (bugGenerator.IsWaveComplete())
        {
            PlayerMoney.money += bugGenerator.currentWave.reward;
            currentWaveNum++;
            
            if (currentWaveNum >= waves.Length)
            {
                WinLoseImage.SetActive(true);
                bugGenerator.isPaused = true;
                isPaused = true;
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
            timerText.text = "00:00";
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
            //timerUI.SetActive(true);
            if(!isPaused){remainingTime -= Time.deltaTime;}
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
        WinLoseImage.SetActive(true);
        bugGenerator.isPaused = true;
        isPaused = true;
        winLoseText.text = "You Lost!!!!!!!";
    }

}