using UnityEngine;
using TMPro;

public class SellCard : MonoBehaviour
{
    // cardStack that is being dragged and colliding with this object
    public CardStack cardStack;
    [SerializeField] TextMeshProUGUI sellPriceText;

    public void ShowPrice(CardStack cs)
    {
        sellPriceText.text = "$" + GetCardStackPrice(cs).ToString();
    }
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

    public void SellCardStack()
    {
        if (cardStack != null)
        {
            SFXManager.Instance.PlaySound(SFXManager.SoundType.CoinGain);
            PlayerMoney.money += GetCardStackPrice(cardStack);
            //Debug.Log(cardStack);
            Destroy(cardStack.gameObject);
            cardStack = null;
        }
    }

    private float GetCardStackPrice(CardStack cs)
    {
        float ret = 0f;
        for (int i=0; i<cs.Cards.Count; i++)
            {
                ret += cs.Cards[i].cardSellPrice;
            }
        return ret;
    }
}
