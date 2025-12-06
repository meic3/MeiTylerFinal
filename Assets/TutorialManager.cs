using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> currentInteractItem;

    private int currentInteractItemIndex = 0;
    private float currentOriginalZNum;

    public const int tutorialNum = 0;

    [SerializeField]
    private GameObject textbox;

    [SerializeField]
    private SpriteRenderer blackSprite; 

    private Vector3 startingPosition;

    private bool inTutorial = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        textbox.SetActive(true);
        blackSprite.enabled = true;
        inTutorial = false;

    }
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        while (inTutorial)
        {
            if (tutorialNum == 0)
            {
                AllowUserToInteract();
                if (Input.GetMouseButtonUp(0))
                {
                    CheckCardPosition();
                }
            }
        }
    }

    void AllowUserToInteract()
    {
        GameObject parent = currentInteractItem[currentInteractItemIndex].transform.parent.gameObject;
        Vector3 pos = parent.transform.position;
        parent.transform.position = new Vector3(pos.x, pos.y, 0);

    }

    void FinishUserInteraction()
    {
        currentInteractItemIndex ++;
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
        
    }

    public void CheckCardPosition()
    {
        
    }
}
