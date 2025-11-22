using System.Collections.Generic;
using UnityEngine;

public class CardPack : MonoBehaviour
{
    private int cardNum = 20;
    public List<GameObject> listedCards;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cardNum == 0)
        {
            Destroy(this);
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        OpenPack();
    }

    void OpenPack()
    {
        int randNum = Random.Range(0, listedCards.Count);
        GameObject pickedCard = listedCards[randNum];
        Instantiate(pickedCard);
        cardNum --;

    }
}
