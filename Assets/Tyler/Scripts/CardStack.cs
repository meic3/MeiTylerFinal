using UnityEngine;
using System.Collections.Generic;

public class CardStack : Collidable
{
    // list of cards in this card stack
    public List<Card> Cards = new List<Card>();
    // all colliders currently colliding with this stack that the stack should not overlap with
    public List<Collidable> overlappingCollidables = new List<Collidable>();
    // reference to the alpaca card at the top of the stack if there is one
    public Alpaca alpaca;



    private BoxCollider2D col;


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
        PushUpdate();
        //pushedThisFrame = false;
    }

    #region click and drag, merging stacks, expanding stacks

    public bool dragging = false;
    private Vector3 offset;

    public enum StackState { Collapsed, Expanded };
    public StackState stackState = StackState.Collapsed;



    public void OnLeftMouseDown()
    {
        if (stackState == StackState.Expanded) return;

        // Record the difference between the objects centre, and the clicked point on the camera plane.
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
        UpdateCardsSortingOrder(dragBase);

        CardStackManager.Instance.stackBeingDragged = this;
    }

    public void OnLeftMouseUp()
    {
        // Stop dragging
        dragging = false;
        UpdateCardsSortingOrder(normalBase);

        CardStackManager.Instance.stackBeingDragged = null;

        // dragging this stack onto another stack merges this stack onto the other stack
        if (overlappingCollidables.Count > 0)
        {
            MergeInto(FindFirstCardStackInOverlappingCollidables());
        }
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


    private int normalBase = 0;
    private int dragBase = 10000;

    public void UpdateCardsSortingOrder(int baseOrder)
    {
        //Debug.Log("sorting: " + gameObject.name + ", " + baseOrder + ", #cards: " + Cards.Count);
        for (int i = 0; i < Cards.Count; i++)
            Cards[i].SetSortingOrder(baseOrder + i);
    }

    // merge this stack into another stack
    private void MergeInto(CardStack cs)
    {
        if (cs == null) return;

        // cannot place cards on top of alpaca
        if (cs.alpaca != null) return;

        foreach (Card card in Cards)
        {
            cs.AddCard(card);
        }
        cs.UpdateCardsSortingOrder(normalBase);

        //collidingCardStack = null;
        Destroy(gameObject);
    }

    // find the first CardStack in the list of colliding colliders
    // returns null if none found
    private CardStack FindFirstCardStackInOverlappingCollidables()
    {
        foreach (Collidable collidable in overlappingCollidables)
        {
            CardStack cs = collidable.gameObject.GetComponent<CardStack>();
            if (cs != null) return cs;
        }

        return null;
    }
    #endregion


    public void AddCard(Card card)
    {
        Cards.Add(card);
        card.transform.SetParent(this.transform);
        card.CurrentStack = this;

        if (card.tag == "alpaca") alpaca = card.GetComponent<Alpaca>();

        // refresh card positions
        if (stackState == StackState.Collapsed) CollapseStack();
        else if (stackState == StackState.Expanded) ExpandStack();
    }

    public void RemoveCard(Card card)
    {
        Cards.Remove(card);

        if (card.tag == "alpaca")
        {
            alpaca = null;
        }

        CollapseStack();
        if (Cards.Count>1) ExpandStack();
    }



    #region push away from nearby colliders

    //public bool pushedThisFrame = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Collidable collidable = col.GetComponent<Collidable>();
        if (collidable == null) return;
        if (!overlappingCollidables.Contains(collidable))
            overlappingCollidables.Add(collidable);
        /*
        CardStack cs = col.GetComponent<CardStack>();
        if (cs != null && !collidingCardStacks.Contains(cs))
            collidingCardStacks.Add(cs);
        */
    }

    private float pushSpd = 3f;
    private void PushUpdate()
    {
        if (overlappingCollidables.Count == 0) return;

        // does not push while being dragged
        if (CardStackManager.Instance.stackBeingDragged == this) return;

        // move away from all colliding objects
        for (int i = 0; i < overlappingCollidables.Count; i++)
        {
            if (overlappingCollidables[i] == null)
            {
                overlappingCollidables.RemoveAt(i);
                continue;
            }

            // does not interact with the stack being dragged
            CardStack cs = overlappingCollidables[i].gameObject.GetComponent<CardStack>();
            if (cs != null && cs == CardStackManager.Instance.stackBeingDragged) continue;
            // does not push away from stacks that have already pushed this frame
            //if (cs != null && cs.pushedThisFrame) continue;

            // move away from the colliding object
            Vector3 dir = (transform.position - overlappingCollidables[i].gameObject.transform.position).normalized;
            if (dir == new Vector3(0, 0, 0)) { dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized; }
            transform.position += dir * pushSpd * Time.deltaTime;
        }
        //pushedThisFrame = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Collidable collidable = col.GetComponent<Collidable>();
        if (collidable == null) return;
        if (overlappingCollidables.Contains(collidable))
        {
            overlappingCollidables.Remove(collidable);

            if (TryGetComponent<CardStack>(out CardStack cs))
            {
                cs.overlappingCollidables.Remove(this);
            }
        }


        /*
        CardStack cs = col.GetComponent<CardStack>();
        if (cs != null && collidingCardStacks.Contains(cs))
            collidingCardStacks.Remove(cs);
        */
    }
    #endregion
}
