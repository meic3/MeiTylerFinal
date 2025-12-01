using UnityEngine;

public class ChainModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] int count = 3;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.stats.chain.AddValue(count);
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.stats.chain.AddValue(-count);
    }
    public void OnUpdate(Alpaca alpaca) { }
}