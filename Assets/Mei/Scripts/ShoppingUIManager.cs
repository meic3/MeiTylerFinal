using UnityEngine;

public class ShoppingUIManager : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 closedPos;
    private Vector3 openPos;


    [SerializeField]
    private RectTransform CardUI;
    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        closedPos = rectTransform.position;
    }
    void Start()
    {

        openPos = new Vector3(closedPos.x - CardUI.sizeDelta.x, closedPos.y, closedPos.z);

    }

    public void openCloseShopping()
    {
        if (isOpen)
        {
            rectTransform.position = closedPos;
        }
        else
        {
            rectTransform.position = openPos;
        }
        isOpen = !isOpen;
    }
}
