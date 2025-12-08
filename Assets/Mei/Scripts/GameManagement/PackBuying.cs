
using UnityEngine;

public class PackBuying : MonoBehaviour
{
    public bool mouseHold = false;
    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector3 origPos = Vector3.zero;

    private PurchaseWidget packData;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        packData = GetComponent<PurchaseWidget>();
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
                rectTransform.position = origPos;
                mouseHold = false;
                PlayerMoney.money -= packData.packPrice;
                }
                else
                {
                    rectTransform.position = origPos;
                    mouseHold = false;
                }

            }
        }
    }

    public void PackDrag()
    {
        mouseHold = true;
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