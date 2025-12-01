using UnityEngine;

public class SellCard : MonoBehaviour
{
    // cardStack that is being dragged and colliding with this object
    public CardStack cardStack;

    private void OnTriggerEnter2D(Collider2D col)
    {
        CardStack cs = col.GetComponent<CardStack>();
        if (cs == CardStackManager.Instance.stackBeingDragged)
        {
            cardStack = cs;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        CardStack cs = col.GetComponent<CardStack>();
        if (cs == cardStack)
        {
            cardStack = null;
        }
    }

    private void Update()
    {
        if (cardStack != null && Input.GetMouseButtonUp(0))
        {
            // sell all cards in card stack
            for (int i=0; i<cardStack.Cards.Count; i++)
            {
                PlayerMoney.money += cardStack.Cards[i].cardPrice;
                //Destroy(cardStack.Cards[i]);
            }
            Destroy(cardStack.gameObject);
            cardStack = null;
        }
    }
}
