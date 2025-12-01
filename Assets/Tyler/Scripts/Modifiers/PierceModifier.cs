using UnityEngine;

public class PierceModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] int count = 3;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.stats.pierce.AddValue(count);
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.stats.pierce.AddValue(-count);
    }
    public void OnUpdate(Alpaca alpaca) { }
}