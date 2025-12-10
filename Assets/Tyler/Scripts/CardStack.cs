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

    private bool audioPlayed = false;

    private BoxCollider2D col;
    private float cardFollowSpeed = 20f;
    // per-card extra local offsets used to create a trailing/lag effect without changing parent
    private List<Vector3> extraLocalOffsets = new List<Vector3>();
    private Vector3 prevPosition;
    private float perCardLagMultiplier = 0.4f;


    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = true;
        prevPosition = transform.position;

        // initialize offsets to match existing cards (if any)
        extraLocalOffsets.Clear();
        for (int i = 0; i < Cards.Count; i++) extraLocalOffsets.Add(Vector3.zero);
    }

    void Update()
    {
        if (dragging)
        {
            if (!audioPlayed)
            {
                SFXManager.Instance.PlaySound(SFXManager.SoundType.CardDrag);
                audioPlayed = true;
            }
            // Move object, taking into account original offset.
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            targetPos.z = transform.position.z;
            transform.position = targetPos;

            // While dragging (collapsed stacks only), make cards lag behind the stack's current position.
            if (stackState == StackState.Collapsed)
            {
                // movement delta since last frame
                Vector3 delta = transform.position - prevPosition;

                // ensure offsets list matches cards count
                while (extraLocalOffsets.Count < Cards.Count) extraLocalOffsets.Add(Vector3.zero);
                while (extraLocalOffsets.Count > Cards.Count) extraLocalOffsets.RemoveAt(extraLocalOffsets.Count - 1);

                for (int i = 0; i < Cards.Count; i++)
                {
                    // when the stack moves by delta, push an opposite offset onto each card so it stays behind
                    float multiplier = 1f + (Cards.Count-1-i) * perCardLagMultiplier;
                    extraLocalOffsets[i] -= delta * multiplier;

                    // decay the extra offset back to zero over time
                    extraLocalOffsets[i] = Vector3.Lerp(extraLocalOffsets[i], Vector3.zero, Time.deltaTime * cardFollowSpeed);

                    // base collapsed local position
                    Vector3 baseLocal = new Vector3(0, i * collapsedOffset, 0);
                    Cards[i].transform.localPosition = baseLocal + extraLocalOffsets[i];
                }
            }
            prevPosition = transform.position;
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
        SFXManager.Instance.PlaySound(SFXManager.SoundType.Click);
        if (stackState == StackState.Expanded) return;

        // Record the difference between the objects center and the clicked point on the camera plane.
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset += new Vector3(0, .1f, 0);
        dragging = true;
        UpdateCardsSortingOrder(dragBase);

        CardStackManager.Instance.SetStackBeingDragged(this);
        
        if (alpaca != null)
        {
            alpaca.rangeIndicator.transform.localScale = new Vector3(alpaca.stats.range.value * 2, alpaca.stats.range.value * 2, 1);
            alpaca.rangeIndicator.SetActive(true);
        }
    }

    public void OnLeftMouseUp()
    {
        SFXManager.Instance.PlaySound(SFXManager.SoundType.CardPlace);
        audioPlayed = false;
        // Stop dragging
        transform.position -= new Vector3(0, .1f, 0);
        dragging = false;
        UpdateCardsSortingOrder(normalBase);

        // Snap cards back into their proper local positions when dragging stops.
        CollapseStack();

        CardStackManager.Instance.ClearStackBeingDragged();

        if (alpaca != null) alpaca.rangeIndicator.SetActive(false);

        // dragging this stack onto another stack merges this stack onto the other stack
        if (overlappingCollidables.Count > 0)
        {
            CardStack cs = FindFirstCardStackInOverlappingCollidables();
            if (cs != null && CanStackOnto(cs))
            {
                StackOnto(cs);
            }
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


    float expandedOffset = -.27f;
    float collapsedOffset = .05f;

    public void ExpandStack()
    {
        stackState = StackState.Expanded;

        //collider.enabled = false;


        // ensure offsets list matches cards
        while (extraLocalOffsets.Count < Cards.Count) extraLocalOffsets.Add(Vector3.zero);
        for (int i = 0; i < Cards.Count; i++)
        {
            extraLocalOffsets[i] = Vector3.zero;
            Cards[i].transform.localPosition = new Vector3(0, i * expandedOffset, 0);
            Cards[i].EnableInteraction(true);
        }
    }

    private void CollapseStack()
    {
        stackState = StackState.Collapsed;

        //collider.enabled = true;

        // ensure offsets list matches cards
        while (extraLocalOffsets.Count < Cards.Count) extraLocalOffsets.Add(Vector3.zero);
        for (int i = 0; i < Cards.Count; i++)
        {
            extraLocalOffsets[i] = Vector3.zero;
            Cards[i].transform.localPosition = new Vector3(0, i * collapsedOffset, 0);
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
    private void StackOnto(CardStack cs)
    {
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
        // keep per-card offsets in sync
        extraLocalOffsets.Add(Vector3.zero);
        card.transform.SetParent(this.transform);
        card.CurrentStack = this;

        if (card.tag == "alpaca") alpaca = card.GetComponent<Alpaca>();

        // refresh card positions
        if (stackState == StackState.Collapsed) CollapseStack();
        else if (stackState == StackState.Expanded) ExpandStack();
    }

    public void RemoveCard(Card card)
    {
        int idx = Cards.IndexOf(card);
        if (idx >= 0 && idx < extraLocalOffsets.Count)
            extraLocalOffsets.RemoveAt(idx);
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






    // cardstackmanager - while card is being dragged, all stack the card can be dragged onto will show outline

    [SerializeField] private SpriteRenderer outline;
    public void ShowOutline(bool b)
    {
        outline.enabled = b;
    }

    // true if this cardstack can stack onto the other cardstack
    public bool CanStackOnto(CardStack other)
    {
        if (other.alpaca != null) return false;
        if (this.alpaca != null && other.alpaca != null) return false;

        if (this.alpaca != null)
        {
            foreach (Card card in other.Cards)
            {
                if (card.TryGetComponent<ICardModifier>(out ICardModifier modifier) &&
                    !this.alpaca.IsCompatibleWithModifier(modifier))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
