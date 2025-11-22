using UnityEngine;
using System.Collections.Generic;

public class CardStack : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();

    public bool dragging = false;
    private Vector3 offset;
    public List<CardStack> collidingCardStacks = new List<CardStack>();
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

        CardStackManager.Instance.stackBeingDragged = this;

        // Record the difference between the objects centre, and the clicked point on the camera plane.
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
        SetSortingBase(dragBase);
    }

    public void OnLeftMouseUp()
    {
        // Stop dragging
        dragging = false;

        CardStackManager.Instance.stackBeingDragged = null;

        // dragging this stack onto another stack merges this stack onto the other stack
        if (collidingCardStacks.Count > 0)
        {
            MergeInto(collidingCardStacks[0]);
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
            //Cards[i].transform.localPosition = new Vector3(0, -(Cards.Count-i-1) * spread, 0);
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
        if (cs != null && !collidingCardStacks.Contains(cs))
            collidingCardStacks.Add(cs);
    }


    private float pushSpd = 1f;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (collidingCardStacks.Count == 0) return;

        // push away from colliding stacks while nothing is being dragged
        for (int i=0; i<collidingCardStacks.Count; i++)
        {
            if (!dragging && !collidingCardStacks[i].dragging)
            {
                // direction away from the other stack
                Vector3 dir = (transform.position - collidingCardStacks[i].transform.position).normalized;
                if (dir == new Vector3(0, 0, 0)) { dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized; }
                transform.position += dir * pushSpd * Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        CardStack cs = col.GetComponent<CardStack>();
        if (cs != null && collidingCardStacks.Contains(cs))
            collidingCardStacks.Remove(cs);
    }

    private void MergeInto(CardStack cs)
    {
        // cannot place cards on top of alpaca cards
        if (cs.Cards[cs.Cards.Count - 1].tag == "alpaca") return;

        foreach (Card card in Cards)
        {
            cs.AddCard(card);
        }
        cs.SetSortingBase(normalBase);

        //collidingCardStack = null;
        Destroy(gameObject);
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
