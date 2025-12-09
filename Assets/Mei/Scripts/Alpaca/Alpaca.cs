using System.Collections.Generic;
using System.Linq;
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
    [HideInInspector] public GameObject rangeIndicator;

    void RangeStart()
    {
        rangeHelper = GetComponentInChildren<EnemyInRangeHelper>();
        rangeIndicator = transform.Find("RangeIndicator").gameObject;
        /*rangeLineRenderer = GetComponentInChildren<LineRenderer>();
        rangeLineRenderer.loop = true;
        rangeLineRenderer.positionCount = segments;*/
    }

    void UpdateRange()
    {
        //Debug.Log("updating range to: " + stats.range.value);
        rangeHelper.SetRadius(stats.range.value);
    }

    /*
    // show range circle

    private int segments = 32;
    LineRenderer rangeLineRenderer;
    public void ShowRange()
    {
        rangeLineRenderer.enabled = true;
        float radius = stats.range.value;
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * (angleStep * i);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            rangeLineRenderer.SetPosition(i, new Vector3(x, y, 0) + transform.position);
        }
    }

    public void HideRange()
    {
        rangeLineRenderer.enabled = false;
    }*/

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


    public bool IsCompatibleWithModifier(ICardModifier modifier)
    {
        return attack.CompatibleModifiers().Contains(modifier.GetType());
    }


}
