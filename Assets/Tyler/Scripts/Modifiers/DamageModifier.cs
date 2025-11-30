using UnityEngine;

public class DamageModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] float multiplier = 2f;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.stats.damage.MultiplyValue(multiplier);
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.stats.damage.MultiplyValue(1/multiplier);
    }
    public void OnUpdate(Alpaca alpaca) { }
}
