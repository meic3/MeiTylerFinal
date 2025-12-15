using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoNameText;
    [SerializeField] TextMeshProUGUI infoDescriptionText;
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject sellUI;

    GraphicRaycaster graphicRaycaster;
    EventSystem eventSystem;

    private void Start()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        // show sell ui while dragging a card stack
        if (CardStackManager.Instance.stackBeingDragged != null)
        {
            shopUI.SetActive(false);
            sellUI.SetActive(true);
        }
        else
        {
            shopUI.SetActive(true);
            sellUI.SetActive(false);
        }

        // show shop info if hovering
        PurchaseWidget purchaseWidget = TryHitPurchaseWidget();
        if (purchaseWidget != null)
        {
            infoNameText.text = purchaseWidget.packObject.GetComponent<CardPack>().name;
            infoDescriptionText.text = CardPack.BuildDescription(
                purchaseWidget.packObject.GetComponent<CardPack>().description,
                purchaseWidget.packObject.GetComponent<CardPack>().cardNum,
                purchaseWidget.packObject.GetComponent<CardPack>().listedCards);

            return;
        }

        // show card pack info if hovering
        CardPack cardPack = TryHitCardPack();
        if (cardPack != null)
        {
            infoNameText.text = cardPack.name;
            infoDescriptionText.text = cardPack.description;
            return;
        }
        
        // show card info if hovering
        Card card = CardStackManager.Instance.GetHoveringCard();
        if (card != null)
        {
            Alpaca alpaca = card.GetComponent<Alpaca>();
            if (alpaca != null)
            {
                infoNameText.text = card.name;
                infoDescriptionText.text = card.description + "\n" + alpaca.stats.ToString();
                return;
            }

            infoNameText.text = card.name;
            infoDescriptionText.text = card.description;
            return;
        }


        infoNameText.text = "";
        infoDescriptionText.text = "";
    }




    public CardPack TryHitCardPack()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);
        return hit.collider != null ? hit.collider.GetComponent<CardPack>() : null;
    }
    public PurchaseWidget TryHitPurchaseWidget()
    {
        //Set up the new Pointer Event
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(pointerEventData, results);
        foreach (RaycastResult result in results)
        {
            PurchaseWidget purchaseWidget = result.gameObject.GetComponent<PurchaseWidget>();
            if (purchaseWidget != null) return purchaseWidget;
        }
        return null;
    }
}
