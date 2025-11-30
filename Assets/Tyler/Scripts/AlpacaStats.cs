using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AlpacaStats
{
    Stat[] stats;
    public Stat damage = new Stat(1);
    public Stat range = new Stat(3);
    public Stat speed = new Stat(1);
    public Stat slow = new Stat(0);// to do: need to clamp value from 0-1
    public Stat attackCount = new Stat(1); // depending on the attack style, could be # of projectile, beam, or trap. each attack is directed at a different bug 
    public Stat chain = new Stat(0); // not implemented
    public Stat cullingHealth = new Stat(0); // not implemented


    public void Recalculate()
    {
        foreach (Stat stat in stats)
        { stat.Recalculate(); }
    }

    public void Start()
    {
        stats = new Stat[]{ damage, range, speed, slow, attackCount, chain, cullingHealth };

        foreach (Stat stat in stats)
        { stat.Start(); }
    }

    [System.Serializable]
    public class Stat
    {
        public float value;
        float baseValue;

        // ---Stat modifiers---
        // multiplicative stats, represent the percentage of the default number
        float mValue = 1;
        // additive stats
        float aValue = 0;

        public Stat(float value)
        {
            this.value = value;
        }

        public int IntValue => Mathf.RoundToInt(value);

        public void MultiplyValue(float factor)
        { mValue *= factor; }

        public void AddValue(float addend)
        { aValue += addend; }
        
        public void Start()
        { baseValue = value; }

        public void Recalculate()
        { value = baseValue * mValue + aValue; }
    }
}
