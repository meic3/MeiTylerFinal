using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

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


    public GameObject CardStackEmpty;

    public LayerMask CardStackLayer;
    public LayerMask CardLayer;

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
            CardStack cardStack = TryHitCardStack();
            if (cardStack != null && cardStack.stackState == CardStack.StackState.Collapsed)
            {
                cardStack.OnLeftMouseUp();
            }
            else
            {
                TryHitCard()?.OnLeftMouseUp();
            }
        }

        if (Input.GetMouseButtonDown(1))
            TryHitCardStack()?.OnRightMouseDown();
        
        if (Input.GetMouseButtonUp(1))
            TryHitCardStack()?.OnRightMouseUp();
    }
    

    public CardStack TryHitCardStack()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, CardStackLayer);

        return hit.collider != null ? hit.collider.GetComponent<CardStack>() : null;
    }

    public Card TryHitCard()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, CardLayer);

        return hit.collider != null ? hit.collider.GetComponent<Card>() : null;
    }
}