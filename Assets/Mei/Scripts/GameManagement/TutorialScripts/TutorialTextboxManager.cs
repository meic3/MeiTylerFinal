using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using Unity.VisualScripting;

public class TutorialTextboxManager : MonoBehaviour
{
    public List<TutorialTextboxInfo> tutorialTextboxInfos;
    public int tutorialTextIndex = 0;

    private int tutorialTextNum;
    public int tutorialTextGroupIndex = 0;

    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    TutorialManager tutorialManager;

    public bool textShow = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialTextNum = GetGroupSize();
        UpdateTextMessage();
    }
    public void NextText()
    {
        tutorialTextIndex ++;

        if (tutorialTextIndex >= tutorialTextNum)
        {
            tutorialManager.NewTutorial();
        }
        else
        {
           UpdateTextMessage();
        }

    }
    public void NextTextGroup()
    {
        tutorialTextNum = GetGroupSize();
        tutorialTextGroupIndex++;
        tutorialTextIndex = 0;
        UpdateTextMessage();
    }

    void UpdateTextMessage()
    {
        dialogueText.text = tutorialTextboxInfos[tutorialTextGroupIndex].Messages[tutorialTextIndex];
    }

    int GetGroupSize()
    {
        int count = tutorialTextboxInfos[tutorialTextGroupIndex].Messages.Count;
        return count;
    }

}
