using UnityEngine;
using System.Collections.Generic;

public class CardStack : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();

    public bool dragging = false;
    private Vector3 offset;
    public CardStack collidingCardStack;
    private BoxCollider2D col;

    public enum StackState { Collapsed, Expanded };
    public StackState stackState = StackState.Collapsed;


    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = true;
    }

    void Update()
    {
        if (dragging)
        {
            // Move object, taking into account original offset.
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }

    public void OnLeftMouseDown()
    {
        if (stackState == StackState.Expanded) return;

        // Record the difference between the objects centre, and the clicked point on the camera plane.
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
        SetSortingBase(dragBase);
    }

    public void OnLeftMouseUp()
    {
        // Stop dragging
        dragging = false;

        // dragging this stack onto another stack merges this stack onto the other stack
        if (collidingCardStack != null)
        {
            MergeInto(collidingCardStack);
            collidingCardStack = null;
            Destroy(gameObject);
        }
        else SetSortingBase(normalBase);

    }

    public void OnRightMouseDown()
    {
        ToggleExpand();
    }

    public void OnRightMouseUp()
    {

    }

    private void ToggleExpand()
    {
        if (stackState == StackState.Collapsed)
            ExpandStack();
        else
            CollapseStack();
    }

    public void ExpandStack()
    {
        stackState = StackState.Expanded;

        //collider.enabled = false;

        float spread = 0.7f;

        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].transform.localPosition = new Vector3(0, -i * spread, 0);
            Cards[i].EnableInteraction(true);
        }
    }

    private void CollapseStack()
    {
        stackState = StackState.Collapsed;

        //collider.enabled = true;

        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].transform.localPosition = new Vector3(0, i * 0.05f, 0);
            Cards[i].EnableInteraction(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CardStack cs = col.GetComponent<CardStack>();
        if (cs != null)
            collidingCardStack = cs;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        CardStack cs = col.GetComponent<CardStack>();
        if (cs == collidingCardStack)
            collidingCardStack = null;
    }

    private void MergeInto(CardStack cs)
    {
        foreach (Card card in Cards)
        {
            cs.AddCard(card);
        }
        cs.SetSortingBase(normalBase);
    }

    public void AddCard(Card card)
    {
        Cards.Add(card);
        card.transform.SetParent(this.transform);
        card.CurrentStack = this;

        if (stackState == StackState.Collapsed) CollapseStack();
        else if (stackState == StackState.Expanded) ExpandStack();
    }

    public void RemoveCard(Card card)
    {
        Cards.Remove(card);
        CollapseStack();
        if (Cards.Count>1) ExpandStack();
    }



    private int normalBase = 0;
    private int dragBase = 10000;

    public void SetSortingBase(int baseOrder)
    {
        //Debug.Log("sorting: " + gameObject.name + ", " + baseOrder);
        for (int i = 0; i < Cards.Count; i++)
            Cards[i].SetSortingOrder(baseOrder + i);
    }
}
