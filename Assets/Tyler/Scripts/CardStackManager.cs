using System;
using UnityEngine;
using TMPro;

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

    public int maxCardStackSize = 5;

    public GameObject newCardStack;

    public LayerMask cardStackLayer;
    public LayerMask cardLayer;

    public CardStack stackBeingDragged;
    public bool showingOutline;

    [SerializeField] Collider2D playArea;

    [SerializeField] TextMeshProUGUI hoveringCardNameText;
    [SerializeField] TextMeshProUGUI hoveringCardDescriptionText;
    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject SellUI;

    public Card GetHoveringCard()
    {
        if (stackBeingDragged != null)
        {
            return stackBeingDragged.Cards[stackBeingDragged.Cards.Count-1];
        }
        CardStack hoveringCardStack = TryHitCardStack();
        if (hoveringCardStack != null)
        {
            if (hoveringCardStack.stackState == CardStack.StackState.Collapsed)
                return hoveringCardStack.Cards[hoveringCardStack.Cards.Count-1];
            else if (hoveringCardStack.stackState == CardStack.StackState.Expanded)
            {
                Card hoveringCard = TryHitCard();
                if (hoveringCard != null)
                    return hoveringCard;
            }
        }
        return null;
    }


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
            }*/
            else
            {
                TryHitCard()?.OnLeftMouseUp();
            }
        }

        if (Input.GetMouseButtonDown(1))
            TryHitCardStack()?.OnRightMouseDown();
        
        if (Input.GetMouseButtonUp(1))
            TryHitCardStack()?.OnRightMouseUp();


        /*
        if (!showingOutline && stackBeingDragged != null)
        {
            ShowCardStacksOutlines(true);
            showingOutline = true;
        }
        else if (showingOutline && stackBeingDragged == null)
        {
            ShowCardStacksOutlines(false);
            showingOutline = false;
        }*/
    }

    public void SetStackBeingDragged(CardStack cs)
    {
        if (cs == null) return;
        stackBeingDragged = cs;
        ShowCardStacksOutlines(true);
        SellUI.GetComponent<SellCard>().ShowPrice(stackBeingDragged);
    }

    public void ClearStackBeingDragged()
    {
        stackBeingDragged = null;
        ShowCardStacksOutlines(false);
        SellUI.GetComponent<SellCard>().SellCardStack();
    }
    

    public CardStack TryHitCardStack()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, cardStackLayer);
        //if (hit.transform != null) Debug.Log(hit.transform.gameObject.name);
        return hit.collider != null ? hit.collider.GetComponent<CardStack>() : null;
    }

    public Card TryHitCard()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, Mathf.Infinity, cardLayer);

        Card topCard = null;
        SpriteRenderer topSR = null;

        foreach (RaycastHit2D hit in hits)
        {
            SpriteRenderer sr = hit.collider.gameObject.transform.Find("CardSprite").GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (topSR == null || sr.sortingOrder > topSR.sortingOrder)
            {
                topSR = sr;
                topCard = hit.collider.GetComponent<Card>();
            }
        }

        return topCard;
    }

    private void ShowCardStacksOutlines(bool b)
    {
        CardStack[] cardStacks = (CardStack[]) FindObjectsByType(typeof(CardStack), FindObjectsSortMode.None);
        foreach (CardStack cardStack in cardStacks)
        {
            if (cardStack == stackBeingDragged) continue;
            if (b && !stackBeingDragged.CanStackOnto(cardStack)) continue;
            
            cardStack.ShowOutline(b);
        }
    }

    public bool IsInPlayArea(CardStack cardStack)
    {
        Bounds b = cardStack.col.bounds;

        return playArea.OverlapPoint(b.min) &&
               playArea.OverlapPoint(b.max);
    }

    public bool IsInPlayArea()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return playArea.OverlapPoint(mouseWorldPos);
    }

    public bool IsInSellArea(CardStack cardStack)
    {
        return SellUI.GetComponent<SellCard>().cardStack != null;
    }
}