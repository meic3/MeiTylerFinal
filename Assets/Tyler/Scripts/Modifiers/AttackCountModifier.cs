using UnityEngine;

public class AttackCountModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] int count = 1;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.stats.attackCount.AddValue(count);
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.stats.attackCount.AddValue(-count);
    }
    public void OnUpdate(Alpaca alpaca) { }
}
