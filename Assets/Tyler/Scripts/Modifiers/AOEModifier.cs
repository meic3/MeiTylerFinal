using UnityEngine;

public class AOEModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] float value = 1;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.stats.AOE.AddValue(value);
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.stats.AOE.AddValue(-value);
    }
    public void OnUpdate(Alpaca alpaca) { }
}