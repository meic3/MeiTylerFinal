using UnityEngine;

public class ShoppingUIManager : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 closedPos;
    private Vector3 openPos;

    public GameObject anchorObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedPos = anchorObj.transform.position;
        openPos = new Vector3 (closedPos.x - 2, closedPos.y, closedPos.z);

    }

    public void openCloseShopping()
    {
        if (isOpen)
        {
            anchorObj.transform.position = closedPos;
        }
        else
        {
            anchorObj.transform.position = openPos;
        }
        isOpen = !isOpen;
    }
}
