using System.Collections.Generic;
using UnityEngine;

public class Alpaca : MonoBehaviour
{
    // multiplicative stats
    // represent the percentage of the default number
    public float mSpeed = 1f;
    public float mDamage = 1f;

    // additive stats
    public float aDamage = 0f;
    public float aRange = 0f;


    Card card;
    CardStack currentStack;
    int cardsCount;

    [SerializeField] CircleCollider2D rangeCol;
    float defaultRange;
    [SerializeField] BulletShooter bulletShooter;
    float defaultSpeed;

    void Start()
    {
        card = GetComponent<Card>();

        defaultRange = rangeCol.radius;
        defaultSpeed = bulletShooter.shootCD;
    }

    void Update()
    {
        CheckForStackUpdate();

        rangeCol.radius = defaultRange + aRange;
        bulletShooter.shootCD = defaultSpeed / mSpeed;
    }


    #region updating modifiers
    // current modifiers on the alpaca
    public List<ICardModifier> activeModifiers = new List<ICardModifier>();

    // refresh modifiers whenever there is a change to the stack
    private void CheckForStackUpdate()
    {
        if (card.CurrentStack != currentStack || currentStack.Cards.Count != cardsCount)
        {
            currentStack = card.CurrentStack;
            cardsCount = currentStack.Cards.Count;
            UpdateAlpacaModifiers();
        }
    }

    // apply all power up cards' modifiers to the alpaca
    private void UpdateAlpacaModifiers()
    {
        DeactivateModifiersOnAlpaca();
        ActivateModifiersOnAlpaca();
    }

    private void DeactivateModifiersOnAlpaca()
    {
        foreach (ICardModifier modifier in activeModifiers)
            modifier.OnDeactivate(this);

        activeModifiers.Clear();
    }

    private void ActivateModifiersOnAlpaca()
    {
        // activate new effects from all cards below the top
        for (int i = 0; i < currentStack.Cards.Count - 1; i++)
        {
            foreach (ICardModifier modifier in currentStack.Cards[i].GetComponents<ICardModifier>())
            {
                modifier.OnActivate(this);
                activeModifiers.Add(modifier);
            }
        }
    }
    #endregion
}
