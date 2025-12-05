using System.Collections.Generic;
using UnityEngine;

public class Alpaca : MonoBehaviour
{
    public AlpacaStats stats;

    IAttack attack;
    float baseAttackCooldown = 1f;
    float cooldownTimer = 0f;

    void Start()
    {
        stats.Start();
        RangeStart();

        card = GetComponent<Card>();
        attack = GetComponent<IAttack>();
    }

    void Update()
    {
        CheckForStackUpdate();

        // attack on cooldown
        if (baseAttackCooldown / stats.speed.value < cooldownTimer)
        {
            // if there are bugs to attack
            if (rangeHelper.bugsInRange.Count > 0)
            {
                attack.Attack(this);
                cooldownTimer = 0f;
            }
        }
        else cooldownTimer += Time.deltaTime;
    }

    #region tracking enemies in range

    // scaling collider based on range
    [HideInInspector] public EnemyInRangeHelper rangeHelper;

    void RangeStart() { rangeHelper = GetComponentInChildren<EnemyInRangeHelper>(); }

    void UpdateRange()
    {
        //Debug.Log("updating range to: " + stats.range.value);
        rangeHelper.SetRadius(stats.range.value);
    }

    #endregion

    #region tracking modifiers

    // current modifiers on the alpaca
    public List<ICardModifier> activeModifiers = new List<ICardModifier>();

    Card card;
    CardStack currentStack;
    int cardsCount;

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

    // clear and reapply modifiers
    private void UpdateAlpacaModifiers()
    {
        DeactivateModifiersOnAlpaca();
        activeModifiers.Clear();

        GetModifiersFromStack();
        ActivateModifiersOnAlpaca();

        stats.Recalculate();
        UpdateRange();
    }

    private void DeactivateModifiersOnAlpaca()
    {
        foreach (ICardModifier modifier in activeModifiers)
            modifier.OnDeactivate(this);
    }

    private void ActivateModifiersOnAlpaca()
    {
        foreach (ICardModifier modifier in activeModifiers)
            modifier.OnActivate(this);
    }

    private void GetModifiersFromStack()
    {
        for (int i = 0; i < currentStack.Cards.Count - 1; i++)
        {
            foreach (ICardModifier modifier in currentStack.Cards[i].GetComponents<ICardModifier>())
            {
                activeModifiers.Add(modifier);
            }
        }
    }
    #endregion
}
