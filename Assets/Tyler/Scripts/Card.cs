using UnityEngine;

public class Card : MonoBehaviour
{
    public string name = "Default Name";
    public string description = "Default Description";
    public float cardSellPrice = 1f;



    SpriteRenderer sr;
    BoxCollider2D col;
    Canvas spriteTextCanvas;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        spriteTextCanvas = GetComponentInChildren<Canvas>();
        //col.enabled = false;

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (CurrentStack == null)
        {
            InstantiateNewCardStack();
        }
    }

    public void SetSortingOrder(int i)
    {
        sr.sortingOrder = i;
        spriteTextCanvas.sortingOrder = i;
    }



    
    public bool canDrag = false;
    private bool dragging = false;
    private Vector3 offset;
    public CardStack CurrentStack;
    private Vector3 initialPos;
    private float leaveStackDistanceThreshold = .5f;

    public void EnableInteraction(bool enable)
    {
        //col.enabled = enable;
        canDrag = enable;
    }

    void Update()
    {
        if (dragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;


            // if dragged far enough away from its current stack
            if (Vector3.Distance(initialPos, transform.position) > leaveStackDistanceThreshold)
            {
                dragging = false;

                // remove from current stack and make it its own stack and keep dragging
                if (CurrentStack != null)
                    CurrentStack.RemoveCard(this);

                CardStack newCardStack = InstantiateNewCardStack();
                newCardStack.OnLeftMouseDown();
            }
        }
    }
    
    public void OnLeftMouseDown()
    {
        //Debug.Log("down");
        if (!canDrag) return;

        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;

        initialPos = transform.position;
    }


    
    public void OnLeftMouseUp()
    {
        //Debug.Log("up");
        if (!canDrag) return;

        dragging = false;

        CurrentStack.ExpandStack();
    }

    // create a new cardStack and add this card to the stack
    private CardStack InstantiateNewCardStack()
    {
        GameObject go = Instantiate(CardStackManager.Instance.newCardStack, transform.position, Quaternion.identity);
        CardStack newCardStack = go.GetComponent<CardStack>();
        newCardStack.AddCard(this);
        return newCardStack;
    }
}
