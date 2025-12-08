
using UnityEngine;
using UnityEngine.UI;

public class PackBuying : MonoBehaviour
{
    public bool mouseHold = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Image image;

    private Vector3 origPos = Vector3.zero;

    private PurchaseWidget packData;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        packData = GetComponent<PurchaseWidget>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (mouseHold)
        {
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                Input.mousePosition,
                canvas.worldCamera,
                out worldPos
            );
            worldPos = new Vector3(worldPos.x, worldPos.y, 0);
            rectTransform.position = worldPos;


            if (Input.GetMouseButtonDown(0))
            {
                if (PlayerMoney.money >= packData.packPrice)
                {
                    GameObject boughtPack = Instantiate(packData.packObject);
                    boughtPack.transform.localPosition = worldPos;
                    PlayerMoney.money -= packData.packPrice;
                }

                rectTransform.position = origPos;
                mouseHold = false;
                image.maskable = true;
                image.RecalculateClipping();
                image.RecalculateMasking();
            }
        }
    }

    public void PackDrag()
    {
        mouseHold = true;
        image.maskable = false;
        image.RecalculateClipping();
        image.RecalculateMasking();
        
        if (origPos == Vector3.zero)
        {
            origPos = rectTransform.position;
            //Debug.Log(origPos);
        }
    }
    
    public void PackRelease()
    {
        mouseHold = false;
    }
}