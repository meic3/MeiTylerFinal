using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> currentInteractItem;

    private int currentInteractItemIndex = 0;
    private int currentOriginalLayerNum;

    [SerializeField]
    private GameObject textbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AllowUserToInteract()
    {
        SpriteRenderer renderer = currentInteractItem[currentInteractItemIndex].GetComponentInChildren<SpriteRenderer>();
        currentOriginalLayerNum = renderer.sortingOrder;
        renderer.sortingOrder = 10;
    }

    void FinishUserInteraction()
    {
        SpriteRenderer renderer = currentInteractItem[currentInteractItemIndex].GetComponentInChildren<SpriteRenderer>();
        renderer.sortingOrder = currentOriginalLayerNum;
        currentInteractItemIndex ++;
    }

    public void NewTutorial()
    {
        textbox.SetActive(false);
        AllowUserToInteract();
    }
}
