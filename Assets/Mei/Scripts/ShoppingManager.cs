using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class ShoppingManager : MonoBehaviour
{
    public GameObject widgetPrefab;
    public List<PurchaseWidget> allWidgets;
    public List<PurchaseWidgetInfo> allPacksInfo;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0;i<allPacksInfo.Count;i++)
        {
            GameObject newPack = Instantiate(widgetPrefab);
            PurchaseWidget packScript = newPack.GetComponent<PurchaseWidget>();
            packScript.Init(allPacksInfo[i].spr, allPacksInfo[i].price, allPacksInfo[i].prefab);
            allWidgets.Add(packScript);
        }
        for(int i = 0;i<allWidgets.Count;i++)
        {
            PurchaseWidget packScript = allWidgets[i].GetComponent<PurchaseWidget>();
            packScript.transform.SetParent(transform);
            float set_y = i * (PurchaseWidget.height + PurchaseWidget.margin);
            packScript.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
            packScript.transform.localScale = new UnityEngine.Vector3(1,1,1);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
