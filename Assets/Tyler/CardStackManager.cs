using System;
using UnityEngine;

public class CardStackManager : MonoBehaviour
{
    public static CardStackManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }


    public GameObject newCardStack;

    public LayerMask cardStackLayer;
    public LayerMask cardLayer;

    public CardStack stackBeingDragged;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CardStack cardStack = TryHitCardStack();
            if (cardStack != null && cardStack.stackState == CardStack.StackState.Collapsed)
            {
                cardStack.OnLeftMouseDown();
            }
            else
            {
                TryHitCard()?.OnLeftMouseDown();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (stackBeingDragged != null)
            {
                stackBeingDragged.OnLeftMouseUp();
            }    
            /*
            CardStack cardStack = TryHitCardStack();
            if (cardStack != null && cardStack.stackState == CardStack.StackState.Collapsed)
            {
                cardStack.OnLeftMouseUp();
            }
            else
            {
                TryHitCard()?.OnLeftMouseUp();
            }*/
        }

        if (Input.GetMouseButtonDown(1))
            TryHitCardStack()?.OnRightMouseDown();
        
        if (Input.GetMouseButtonUp(1))
            TryHitCardStack()?.OnRightMouseUp();
    }
    

    public CardStack TryHitCardStack()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, cardStackLayer);
        if (hit.transform != null) Debug.Log(hit.transform.gameObject.name);
        return hit.collider != null ? hit.collider.GetComponent<CardStack>() : null;
    }

    public Card TryHitCard()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, cardLayer);

        return hit.collider != null ? hit.collider.GetComponent<Card>() : null;
    }
}