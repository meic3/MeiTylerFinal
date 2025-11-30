using UnityEngine;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour
{
    public class Wave
    {
        public int cockroachNum;
        public int flyNum;
        public int stinkBugNum;
        public Wave(int cockroach, int fly, int stinkbug)
        {
            cockroachNum = cockroach;
            flyNum = fly;
            stinkBugNum = stinkbug;
        }
    }

    public Wave wave1 = new Wave(20,0,0);
    public int waveCount;
    public bool isInGame;
    public BugGenerator bugGenerator;

    [SerializeField]
    private TextMeshProUGUI timerText;
    private float remainingTime;
    private float setTime = 10;

    [SerializeField]
    private GameObject timerUI;

    void Start()
    {
        bugGenerator.canGenerate = false;
        isInGame = false;
        remainingTime = setTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime <= 0)
        {
            StartWave();
        }
        if (!isInGame)
        {
        timerUI.SetActive(true);
        remainingTime -= Time.deltaTime;
        }  
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    public void StartWave()
    {
        timerUI.SetActive(false);
        isInGame = true;
        remainingTime = setTime;
        bugGenerator.canGenerate = true;
    }
}
