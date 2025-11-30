using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseWidget : MonoBehaviour
{
    public float packPrice;
    [SerializeField]
    private Image packPackaging;
    [SerializeField]
    private Button packButton;
    [SerializeField]
    private TMP_Text packPriceText; 

    [SerializeField]
    public GameObject packObject;


    //positions
    public const float height = 10;
    public const float margin = 150;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Sprite spr, float price, GameObject prefab)
    {
        packPrice = price;
        packPackaging.sprite = spr;
        packPriceText.text = (string)"$"+packPrice;
        packObject = prefab;
        
    }

    public void SpawnPack()
    {
        Instantiate(packObject);
    }
}
