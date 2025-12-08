using System;
using UnityEngine;

public interface IAttack
{
    void Attack(Alpaca alpaca);
    Type[] CompatibleModifiers();
}
