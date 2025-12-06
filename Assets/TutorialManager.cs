using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> currentInteractItem;

    [SerializeField] 
    private List<Transform> cardTargets;

    [SerializeField] 
    private float targetRadius = 1f;

    private int currentInteractItemIndex = 0;

    private int currentTargetIndex = 0;
    private float currentOriginalZNum;

    public int tutorialNum = 0;

    [SerializeField]
    private GameObject textbox;

    [SerializeField]
    private SpriteRenderer blackSprite; 

    private Vector3 startingPosition;

    private bool inTutorial = true;

    [SerializeField]
    private PhaseManager phaseManager;

    [SerializeField]
    private BugGenerator bugGenerator;

    public TutorialTextboxManager tutorialTextboxManager;

    [SerializeField]
    private GameObject panelThatCoversAllUI;

    [SerializeField]
    private GameObject panelThatCoversShopUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        textbox.SetActive(true);
        blackSprite.enabled = true;
        inTutorial = false;
        panelThatCoversShopUI.SetActive(false);
        panelThatCoversShopUI.SetActive(true);

    }
    void Start()
    {
        ResetInteractValues();

    }

    // Update is called once per frame
    void Update()
    {
        if (inTutorial)
        {
            if (tutorialNum == 0)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    CheckCardPosition();
                }
            }
            if (tutorialNum == 1)
            {
                panelThatCoversAllUI.SetActive(false);
                panelThatCoversShopUI.SetActive(true);
                phaseManager.isPaused = false;
                bugGenerator.isPaused = false;
                inTutorial = true;
                if(bugGenerator.deadBug == 1)
                {
                    panelThatCoversAllUI.SetActive(true);
                    panelThatCoversShopUI.SetActive(false);
                    phaseManager.isPaused = true;
                    bugGenerator.isPaused = true;
                    FinishUserInteraction();
                }
            }
            if (tutorialNum == 2)
            {
                inTutorial = true;
                if (Input.GetMouseButtonUp(0))
                {
                    currentTargetIndex++;
                    CheckCardPosition();
                }
            }
        } 
        else
        {
            panelThatCoversAllUI.SetActive(true);
        }
    }

    void AllowUserToInteract()
    {
        GameObject parent = currentInteractItem[currentInteractItemIndex].transform.parent.gameObject;
        Vector3 pos = parent.transform.position;
        parent.transform.position = new Vector3(pos.x, pos.y, 0);
        startingPosition = parent.transform.position;

    }

    void FinishUserInteraction()
    {
        tutorialNum ++;
        textbox.SetActive(true);
        blackSprite.enabled = true;
        inTutorial = false;
        //currentInteractItemIndex ++;
        tutorialTextboxManager.NextTextGroup();
    }

    void ResetInteractValues()
    {
        
        for(int i = 0; i < currentInteractItem.Count; i++)
        {
            GameObject parent = currentInteractItem[i].transform.parent.gameObject;
            Vector3 pos = parent.transform.position;
            parent.transform.position = new Vector3(pos.x, pos.y, 10);
        }
    }

    public void NewTutorial()
    {
        textbox.SetActive(false);
        blackSprite.enabled = false;
        ResetInteractValues();
        inTutorial = true;
        AllowUserToInteract();
    }

    public void CheckCardPosition()
    {
        float dist = Vector3.Distance(currentInteractItem[currentInteractItemIndex].transform.parent.gameObject.transform.position, cardTargets[currentTargetIndex].position);

        if (dist <= targetRadius)
        {
            FinishUserInteraction();
            
        }
        else
        {
        
            currentInteractItem[currentInteractItemIndex].transform.parent.gameObject.transform.position = startingPosition;
        }
    

    }
}
