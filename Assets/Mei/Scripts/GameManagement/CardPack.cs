using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CardPack : MonoBehaviour
{
    private int cardNum = 5;
    private int cardCount = 0;
    public List<GameObject> listedCards;

    public float cardDistFromPack = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cardNum <= cardCount)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        OpenPackTest();
    }

    void OpenPack()
    {
        int randNum = UnityEngine.Random.Range(0, listedCards.Count);
        GameObject pickedCard = listedCards[randNum];
        Instantiate(pickedCard);
        cardCount ++;

    }

    void OpenPackTest()
    {
        cardCount ++;
        var radians = 2* MathF.PI / cardNum * cardCount;
        Debug.Log(radians);

        var vertical = MathF.Sin(radians);
        var horizontal = MathF.Cos(radians);

        var cardDir = new Vector3 (horizontal, vertical, 0);

        var cardPos = transform.position + cardDir * cardDistFromPack;
        int randNum2 = UnityEngine.Random.Range(0, listedCards.Count);
        GameObject pickedCard = listedCards[randNum2];
        Instantiate(pickedCard, cardPos, Quaternion.identity);
        

    }
}
