using System.Collections.Generic;
using UnityEngine;

public class TutorialTextboxManager : MonoBehaviour
{
    public List<TutorialTextboxInfo> tutorialTextboxInfos;
    public int tutorialTextIndex = 0;

    public bool textShow = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NextText()
    {
        tutorialTextIndex++;
    }
}
